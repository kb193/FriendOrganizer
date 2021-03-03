Imports System
Imports System.Data.Entity
Imports System.Data.Entity.Migrations
Imports System.Linq
Imports Model

Namespace Migrations

    Friend NotInheritable Class Configuration
        Inherits DbMigrationsConfiguration(Of FinalDbContext)

        Public Sub New()
            AutomaticMigrationsEnabled = False
        End Sub

        Protected Overrides Sub Seed(context As FinalDbContext)
            context.Friends.AddOrUpdate(Function(i) i.FirstName, New [Friend] With {.FirstName = "Kurt", .LastName = "Bartle", .Email = "kurt.bartle@gmail.com"},
                                                     New [Friend] With {.FirstName = "Becky", .LastName = "Allard", .Email = "becky.allard40@yahoo.co.uk"})

            context.ProgrammingLanguages.AddOrUpdate(Function(i) i.Name, New ProgrammingLanguage With {.Name = "C#"},
                                                      New ProgrammingLanguage With {.Name = "TypeScript"},
                                                      New ProgrammingLanguage With {.Name = "F#"},
                                                      New ProgrammingLanguage With {.Name = "Java"})

            context.SaveChanges()

            context.PhoneNumbers.AddOrUpdate(Function(i) i.Number, New FriendPhoneNumber With {.Number = "+44 7951245074", .FriendId = context.Friends.First.Id})


            context.Meetings.AddOrUpdate(Function(i) i.Title, New Meeting With
                                         {.Title = "Watching Football",
                                         .DateFrom = New DateTime(2020, 5, 31),
                                         .DateTo = New DateTime(2020, 5, 31),
                                         .Friends = New List(Of [Friend]) From {context.Friends.Single(Function(j) j.FirstName = "Kurt" And j.LastName = "Bartle")}})
        End Sub

    End Class

End Namespace
