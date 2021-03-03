Imports DataAccess
Imports Model

Public Interface IProgrammingLanguageRepository
    Inherits IGenericRepository(Of ProgrammingLanguage)
    Function IsReferencedByFriendAsync(languageId As Integer) As Task(Of Boolean)
End Interface
