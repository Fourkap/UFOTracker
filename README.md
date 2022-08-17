# UfoTracker

Ufo Tracker est un ensemble d'applications développée en C#(.NET) qui permet la déclaration d'un OVNI ou de PAN(phénomènes aérospatiaux non identifiés).
Ces applications permettent également le suivi de ces phenomènes a parcourir autour de cartes.

---

## Data

La data utilisée dans ces aplications provient du NUFORC (National UFO Reporting Center) elle comprend un peu moins de 90 000 témoignages recesner par le NUFORC. Ces data seront completer par celle du GEIPAN (Groupe d'études et d'informations sur les phénomènes aérospatiaux non identifiés) dans un futur proches.<br>
Data du NUFORC a télécharger ici:  [ufo.json](ufo.json)

![DataNavicat](ImgForDoc/data_navicat.png)

--- 
## Architecture

![ArchitectureUfotracker](ImgForDoc/Architecture_UfoTracker.png)

La partie base de données se repose sur MongoDB.<br> 
La solution .NET regroupe 4 entitées. 
- UfoTrackerModels
- UfoTrackerApi
- UfoTrackerWeb
- UfoTracker Android
--- 


## UfoTracker.Models

UfoTracker Models est une bibliothèque de classe C# .NET qui permet d'utiliser les classes dans l'ensemble des projets constituants la solution. 
Elle est compser de deux modèles:
- Ufo.cs

``` C#
public class Ufo
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("datetime")]
        public DateTime? DateAndTime { get; set; }
        [BsonElement("city")]
        public string? City { get; set; }
        [BsonElement("state")]
        public string? State { get; set; }
        [BsonElement("country")]
        public string? Country { get; set; }
        [BsonElement("shape")]
        public string? Shape { get; set; }
        [BsonElement("comments")]
        public string? Comments { get; set; }
        [BsonElement("date posted")]
        public DateTime? DatePosted { get; set; }
        [BsonElement("latitude")]
        public string? Latitude { get; set; }
        [BsonElement("longitude")]
        public string? Longitude { get; set; }
    }
```
- PageMongo.cs
``` C#
public class PageMongo
    {
        public int Count { get; set; }

        public int Page { get; set; }

        public int Size { get; set; }

        public IEnumerable<Ufo> Ufos { get; set; }
    }
```

--- 
## UfoTracker.API

UfoTracker Api est une Api Rest développer en C# .NET qui est en charge de communiquer avec la base MongoDb et qui permet de fournir et de récuperer l'ensemble des données des applications web et mobile.

Page Mongo permet d'avoir une pagination dans le GetAll.

![Api](ImgForDoc/API.png)

--- 
## UfoTracker.WEB

UfoTracker Web est une application Web ASP.NET qui s'appuye sur UfoTracker.API et UfoTracker.Models. Elle dispose de plusieurs features que nous allons parcourir.

![Web_Home](ImgForDoc/Web_Home.png)

### Carte 
La carte utilise l'api de google maps. elle affiche des markers correspondant a la page. 
les icones des markers sont spécifique selon la forme de l'Ufo aperçu.  
![Web_Home](ImgForDoc/Web_Map.mp4)