Imports System
Imports System.Data.Entity.Migrations
Imports Microsoft.VisualBasic

Namespace Migrations
    Public Partial Class InitialDb
        Inherits DbMigration
    
        Public Overrides Sub Up()
            CreateTable(
                "dbo.Friend",
                Function(c) New With
                    {
                        .Id = c.Int(nullable := False, identity := True),
                        .FirstName = c.String(nullable := False, maxLength := 50),
                        .LastName = c.String(maxLength := 50),
                        .Email = c.String(),
                        .FavouriteLanguageId = c.Int(nullable := False),
                        .RowVersion = c.Binary(nullable := False, fixedLength := true, timestamp := True, storeType := "rowversion")
                    }) _
                .PrimaryKey(Function(t) t.Id) _
                .ForeignKey("dbo.ProgrammingLanguage", Function(t) t.FavouriteLanguageId, cascadeDelete := True) _
                .Index(Function(t) t.FavouriteLanguageId)
            
            CreateTable(
                "dbo.ProgrammingLanguage",
                Function(c) New With
                    {
                        .Id = c.Int(nullable := False, identity := True),
                        .Name = c.String(nullable := False, maxLength := 50)
                    }) _
                .PrimaryKey(Function(t) t.Id)
            
            CreateTable(
                "dbo.Meeting",
                Function(c) New With
                    {
                        .Id = c.Int(nullable := False, identity := True),
                        .Title = c.String(),
                        .DateFrom = c.DateTime(nullable := False),
                        .DateTo = c.DateTime(nullable := False)
                    }) _
                .PrimaryKey(Function(t) t.Id)
            
            CreateTable(
                "dbo.FriendPhoneNumber",
                Function(c) New With
                    {
                        .Id = c.Int(nullable := False, identity := True),
                        .Number = c.String(nullable := False),
                        .FriendId = c.Int(nullable := False)
                    }) _
                .PrimaryKey(Function(t) t.Id) _
                .ForeignKey("dbo.Friend", Function(t) t.FriendId, cascadeDelete := True) _
                .Index(Function(t) t.FriendId)
            
            CreateTable(
                "dbo.MeetingFriend",
                Function(c) New With
                    {
                        .Meeting_Id = c.Int(nullable := False),
                        .Friend_Id = c.Int(nullable := False)
                    }) _
                .PrimaryKey(Function(t) New With { t.Meeting_Id, t.Friend_Id }) _
                .ForeignKey("dbo.Meeting", Function(t) t.Meeting_Id, cascadeDelete := True) _
                .ForeignKey("dbo.Friend", Function(t) t.Friend_Id, cascadeDelete := True) _
                .Index(Function(t) t.Meeting_Id) _
                .Index(Function(t) t.Friend_Id)
            
        End Sub
        
        Public Overrides Sub Down()
            DropForeignKey("dbo.FriendPhoneNumber", "FriendId", "dbo.Friend")
            DropForeignKey("dbo.MeetingFriend", "Friend_Id", "dbo.Friend")
            DropForeignKey("dbo.MeetingFriend", "Meeting_Id", "dbo.Meeting")
            DropForeignKey("dbo.Friend", "FavouriteLanguageId", "dbo.ProgrammingLanguage")
            DropIndex("dbo.MeetingFriend", New String() { "Friend_Id" })
            DropIndex("dbo.MeetingFriend", New String() { "Meeting_Id" })
            DropIndex("dbo.FriendPhoneNumber", New String() { "FriendId" })
            DropIndex("dbo.Friend", New String() { "FavouriteLanguageId" })
            DropTable("dbo.MeetingFriend")
            DropTable("dbo.FriendPhoneNumber")
            DropTable("dbo.Meeting")
            DropTable("dbo.ProgrammingLanguage")
            DropTable("dbo.Friend")
        End Sub
    End Class
End Namespace
