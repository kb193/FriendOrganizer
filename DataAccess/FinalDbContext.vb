Imports System.Data.Entity
Imports System.Data.Entity.ModelConfiguration.Conventions

Public Class FinalDbContext
    Inherits DbContext

    Sub New()
        MyBase.New("FinalFOApp")
    End Sub

    Public Property Friends As DbSet(Of Model.Friend)

    Public Property ProgrammingLanguages As DbSet(Of Model.ProgrammingLanguage)

    Public Property PhoneNumbers As DbSet(Of Model.FriendPhoneNumber)

    Public Property Meetings As DbSet(Of Model.Meeting)


    Protected Overrides Sub OnModelCreating(modelBuilder As DbModelBuilder)
        MyBase.OnModelCreating(modelBuilder)
        modelBuilder.Conventions.Remove(Of PluralizingTableNameConvention)()
    End Sub
End Class
