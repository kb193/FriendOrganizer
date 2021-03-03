Imports Model
Imports DataAccess
Imports System.Data.Entity

Public Class ProgrammingLanguageRepository
    Inherits GenericRepository(Of ProgrammingLanguage, FinalDbContext)
    Implements IProgrammingLanguageRepository

    Public Sub New(context As FinalDbContext)
        MyBase.New(context)
    End Sub

    Public Async Function IsReferencedByFriendAsync(languageId As Integer) As Task(Of Boolean) Implements IProgrammingLanguageRepository.IsReferencedByFriendAsync
        Return Await Context.Friends.AsNoTracking.AnyAsync(Function(i) i.FavouriteLanguageId = languageId)
    End Function

End Class
