Imports Model

Public Interface IProgrammingLanguageLookupDataService
    Function GetProgrammingLanguageLookupAsync() As Task(Of IEnumerable(Of LookupItem))
End Interface
