Imports System
Imports System.Data.Entity.Migrations
Imports Microsoft.VisualBasic

Namespace Migrations
    Public Partial Class test
        Inherits DbMigration
    
        Public Overrides Sub Up()
            DropForeignKey("dbo.Friend", "FavouriteLanguageId", "dbo.ProgrammingLanguage")
            DropIndex("dbo.Friend", New String() { "FavouriteLanguageId" })
            AlterColumn("dbo.Friend", "FavouriteLanguageId", Function(c) c.Int())
            CreateIndex("dbo.Friend", "FavouriteLanguageId")
            AddForeignKey("dbo.Friend", "FavouriteLanguageId", "dbo.ProgrammingLanguage", "Id")
        End Sub
        
        Public Overrides Sub Down()
            DropForeignKey("dbo.Friend", "FavouriteLanguageId", "dbo.ProgrammingLanguage")
            DropIndex("dbo.Friend", New String() { "FavouriteLanguageId" })
            AlterColumn("dbo.Friend", "FavouriteLanguageId", Function(c) c.Int(nullable := False))
            CreateIndex("dbo.Friend", "FavouriteLanguageId")
            AddForeignKey("dbo.Friend", "FavouriteLanguageId", "dbo.ProgrammingLanguage", "Id", cascadeDelete := True)
        End Sub
    End Class
End Namespace
