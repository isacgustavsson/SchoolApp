# Arkitekturövning och refaktorering

> OBS: Som vanligt kan du lösa denna uppgift genom att be någon annan att göra den åt dig. Din lärare, din kompis eller en LLM. Men då kan du lika strunta i att göra den. Det är vad du lär dig på att göra denna uppgift som är poängen, inte det färdiga resultatet. Så, gör den själv, och gör den ordentligt!

Övningen går ut på att förändra en befintlig kodbas, från en hårt kopplad och sammanhållen kod som är svår att utöka och underhålla, till en lösare kopplad och mer modulär kod som är enklare att utöka och underhålla. Slutresultatet kommer inte heller vara perfekt (inge kod är), men det är en övning i att strukturera om kod med hjälp av interfaces, förstå hur decoupling kan se ut i praktiken, samt att flytta runt kod med bibehållen funktionalitet.

Vi kommer göra detta steg för steg:

1. **Nuläget:** Stor klump av kod där applikationslogik och UI är starkt kopplade. Det är inte möjligt att enkelt lägga till ett WebAPI tex, utan att duplicera kod.
2. **Skapa en struktur:** Börja dela upp koden i tydliga ansvarsområden. Skapa mappar.
3. **Skapa namespaces och interfaces** för ökad löst koppling och möjlighet att byta ut implementationer för testning.
3. **Separera till olika projekt** med tydliga dependencies.
4. **Strukturera om dependencies** från lagerstruktur till clean/onion-arkitektur.
5. **WebApi**. Lägg till ett WebAPI-projekt som använder sig av samma logik och datalager som konsolapplikationen.
6. **Lägg till funktionalitet** för att sätta betyg på en viss student för en viss kurs.

Applikationen har fyra och endast fyra funktioner. Håll det till det:
* Lägga till studenter
* Lägga till kurser
* Registrera en student på en kurs
* Hämta/visa alla studenter och kurser

## Steg 1 - Separation of concern
Första steget blir att separera användargränssnittet från applikationslogiken. Vi kommer göra detta genom att skapa en ny klass som hanterar användargränssnittet, och som använder sig av en annan klass för att hantera applikationslogiken (En service-klass).

* Skapa klassen **SchoolService** som ska hantera applikationslogiken. Den ska i sin konstruktor ta emot en instans av **SchoolDbContext** och använda denna för att hämta och spara data. Denna klass ska innehålla dessa metoder:
```csharp
List<Student> GetAllStudents()
List<Course> GetAllCourses()
void AddStudent(Student student)
void AddCourse(Course course)
void EnrollStudent(int studentId, int courseId)
```
* Skapa klassen **UI** som ska hantera användargränssnittet. Den skall i sin konstruktor ta emot en instans av **SchoolService** och använda denna för att hämta data och utföra funktioner. Metoderna i den kan återspegla de metoder som redan finns i **Program.cs**. Den bör också ha någon sorts `Run()`-metod som kör själva while()-loopen.
* Skapa mapparna **UI**, **Core** och **Infrastructure** och flytta UI-klassen till UI-mappen, SchoolService till Core-mappen samt SchoolDbContext till Infrastructure-mappen.

**OBS!** Student och Course kan du lägga i Infrastructure-mappen för tillfället.

* I **Program.cs** skapar du instanser av **SchoolDbContext**, **SchoolService** och **UI** och skickar in SchoolService i UI och SchoolDbContext i SchoolService. Anropa sedan UI-klassens Run-metod.

Du har nu skapat ett första steg i en klassisk lagerstruktur (dvs inte onion/clean). Men det finns fortfarande ingenting som hindrar att UI-klassen använder sig av SchoolDbContext direkt till exempel. Service-klassen är också fortfarande hårt kopplad till Entity Framework. Och våra Studenter och kurser ligger i databaslagret. Detta är saker vi kommer att åtgärda i kommande steg.

**OBS:** En intressant sak att fundera på i detta läge är hur och vad `EnrollStudent` ska rapportera sina tre olika fel som kan uppstå.

## Steg 2 - Namespaces

All kod är nu uppdelad i tre olika klasser, med tydliga ansvarsområden. Vi kan dock göra mer för att göra denna struktur tydligare! Ett första steg är att lägga till namespaces för att tydliggöra vilka klasser som hör ihop.

* Skapa namespaces för klasserna i de olika mapparna. Använd `SchoolApp.UI`, `SchoolApp.Core` och `SchoolApp.Infrastructure`. Exempelvis `namespace SchoolApp.UI;` högst upp i UI-klassen.
* Se till så att rätt using-statements finns i de olika klasserna:
    * **UI:** `using SchoolApp.Core;`
    * **Service-klassen:** `using SchoolApp.Infrastructure;`
    * **Program.cs:** `using SchoolApp.UI; using SchoolApp.Core; using SchoolApp.Infrastructure;` **OBS ALLA TRE BEHÖVS HÄR**

Detta ger oss en första hint om hur saker hänger ihop; vilka klasser använder vilka namespaces.

## Steg 3 - Lagerarkitektur

Nu har vi lite mer tydlighet och ordning och reda! Men, det finns fortfarande mycket att göra. Det finns i nuläget fortfarande ingenting som säger att vi inte kan använda UI-relaterad kod i Service-klassen, eller i klasser som ligger i Infrastructure. Dvs, det GÅR att göra, koden och strukturen stoppar oss inte. Vi kan tydligare tvinga fram denna struktur genom att separera vårt projekt till flera olika projekt.
Vi kommer att skapa tre olika separata projekt: Ett för **UI**, ett för **Core** och ett för **Infrastructure**.

I ditt projekt kör dessa kommandon för att skapa de nya projekten:
```
dotnet new console -o UI
```
* Flytta UI-klassen och Program.cs till UI-projektet och ta bort UI-katalogen.

```
dotnet new classlib -o Core
```
* Flytta Core-klasserna till Core-projektet och ta bort Core-mappen.

```
dotnet new classlib -o Infrastructure
```
* Flytta Infrastructure-klassen till SchoolInfrastructure-projektet och ta bort Infrastructure-mappen.
* Ta bort `SchoolApp.sln`
* Ersätt innehåller i `UI.csproj` med innehållet i `SchoolApp.csproj` 

Nu har vi tre helt separerade projekt som inte vet något om varandra. Koden i UI kommer inte att gå att köra! Nu är det dags att koppla ihop våra projekt så att det fortsätter att fungera som innan, men med en tydligare separation.

* Skapa en ny solution: `dotnet new sln`. Denna fil ska ligga i root-katalogen för hela projektet.
* Högerklicka på sln-filen oc välj **Open Solution**
* Högerklicka på **SchoolApp** i Solution Explorer och välj **Add Existing Project...**. Leta reda på de tre .csproj-filerna i de tre projekten och lägg till dem, så att du ser UI, Core och Infrastructure i Solution Explorer.

Nu ska vi knyta ihop dem på rätt sätt: UI använder ju både Service-klassen och DbContext-klassen så det projektet måste känna till de andra två projekten.

* Högerklicka på **UI** i Solution Explorer och välj **Add Reference...**. Klicka på **Core** i menyn som kommer upp. Gör det igen och lägg till **Infrastructure**. Det har nu lagts till referenser i UI.csproj-filen till de andra projekten.
* Gör samma sak för **Core**, låt den referera till **Infrastructure**.
* **Infrastructure** är det sista lagret i vår arkitektur så den behöver inte veta något mer.

Prova att köra projektet i terminalen (glöm inte att gå in i UI-katalogen med `cd ui`), om allt gått rätt ska det funka precis som innan. Du kan nu också prova att försöka referera till någon kod i UI eller Core från Infrastructure-kod, det kommer inte att gå utan ger ett kompileringsfel.

Din katalogstruktur borde nu se ut så här:

```
- SchoolApp.sln
- UI
    - UI.csproj
    - Program.cs
    - UI.cs
- Core
    - Core.csproj
    - SchoolService.cs
- Infrastructure
    - Infrastructure.csproj
    - SchoolDbContext.cs
    - Student.cs
    - Course.cs
```

## Steg 4 - Onion SchoolApp
Det är inte helt bra att vi har våra Students och Courses i datalagret! De borde vara med i kärnan av vårt program, så att vi tex kan testa det utan att bry oss om hur vi lagrar saker. Låt oss strukturera om till en struktur som påminner mer om Clean SchoolApp, eller Onion SchoolApp. 
För att göra det skall vi vända på beroendet mellan infrastruktur och Core. 

* Ta bort beroendet från **Core** till **Infrastructure** genom att ta bort projekt-referensen i `Core.csproj`. Lägg sedan istället till ett beroende från **Infrastructure** till **Core**. Nu pekar både **UI** och **Infrastructure** till Core-projektet, men Core-projektet vet inget om vare sig UIn eller Databaser!
* Flytta nu `Student.cs` och `Course.cs` till **Core**-projektet och uppdatera deras namespaces.

Nu kommer du att upptäcka att DbContexten som används i Service-klassen inte längre funkar. Den ligger ju i Infrastructure och dit har vi inte längre någon koppling! Det är nu tricket med Onion-arkitekturen kommer in. I **Core** måste vi nu definiera repository-interfaces som Service-klassen kan använda sig av i stället för direkt access till DbContext. 
(För inspiration till att skriva Repositories: https://deviq.com/design-patterns/repository-pattern)

* Exempel på ett generiskt repository med implementation kan du hitta [här]().
* Kolla i **Service-klassen** på alla de platser DbContext-objektet används. Kolla efter vad den används till, vilken data skickas in och vilken data kommer tillbaka. Skapa ett interface som har metoder som matchar denna in- och ut-data.
* Använd detta interface istället som input i konstruktorn. Så, nu är beroendet till Infrastructure borta!
* Nästa steg blir att på **Infrastructure**-sidan skriva implementationen av detta interface, som använder sig av DbContext!
* Det är denna konkreta implementation som sedan kommer att skickas in i service-klassen i **Program.cs** istället för nuvarande DbContext!

Mycket kan krångla i detta steg!
* Var ska databas-filen ligga? Jo, bäst i **root-mappen** (inte i infrastrukturmappen)
* Du kan behöva ändra sökvägen i DbContext-filen till `"Data Source=../school.db"`

Din katalogstruktur borde nu se ut så här:

```
- SchoolApp.sln
- UI
    - UI.csproj
    - Program.cs
    - UI.cs 
- Core
    - Core.csproj
    - SchoolService.cs
    - Student.cs
    - Course.cs
    - IStudentRepository.cs
- Infrastructure
    - Infrastructure.csproj
    - SchoolDbContext.cs
    - StudentRepository.cs
```

## Steg 5 - Ta dig en funderare
Här kan det vara bra att stanna upp ett tag och ta sig en funderare. 
* Ligger kod där den borde ligga? 
* Finns det dupliceringar som skulle kunna fixas? 
* Vad har saker och ting för returvärden, vad skickas runt? 
* Var sker validering av data osv? 

Läs gärna denna artikel och fundera sedan vidare på någon eventuell justering: https://martinfowler.com/bliki/AnemicDomainModel.html 

## Steg 6 - Lägg till ett WebAPI eller MVC
Skapa ett nytt projekt av exempelvis **WebAPI**-typ. 
* Hur kan du göra en endpoint (med minimal API tex) för att lägga till en ny student? 
* Du ska bara behöva använda dig av **Infrastruktur**-lagret och **Logik/Core**-lagret och lägga till en student via serviceklassens `AddStudent`-metod. 
* Du kommer att behöva lägga in DbContexten i DI-containern tillsammans med Repositories och Serviceklass.

