## Quiz App

Run this command to create migration
``` powershell
dotnet ef migrations add [migration_name] --verbose --project QuizApp.Database --startup-project QuizApp.Server
```

Run this to apply migration
``` powershell 
dotnet ef database update --verbose --project QuizApp.Database --startup-project QuizApp.Server
```

