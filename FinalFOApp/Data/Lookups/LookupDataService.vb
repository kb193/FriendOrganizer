Imports System.Data.Entity
Imports DataAccess
Imports Model

Public Class LookupDataService
    Implements IFriendLookupDataService, IMeetingLookupDataService, IProgrammingLanguageLookupDataService
    Public ReadOnly ContextCreator As Func(Of FinalDbContext)

    Sub New(contextCreator As Func(Of FinalDbContext))
        Me.ContextCreator = contextCreator
    End Sub

    Public Async Function GetFriendLookupAsync() As Task(Of IEnumerable(Of LookupItem)) Implements IFriendLookupDataService.GetFriendLookupAsync
        Using ctx = ContextCreator()
            Return Await ctx.Friends.AsNoTracking.Select(Function(i) New LookupItem With {.Id = i.Id, .DisplayMember = i.FirstName & " " & i.LastName}).ToListAsync
        End Using
    End Function

    Public Async Function GetProgrammingLanguageLookupAsync() As Task(Of IEnumerable(Of LookupItem)) Implements IProgrammingLanguageLookupDataService.GetProgrammingLanguageLookupAsync
        Using ctx = ContextCreator()
            Return Await ctx.ProgrammingLanguages.AsNoTracking.Select(Function(i) New LookupItem With {.Id = i.Id, .DisplayMember = i.Name}).ToListAsync
        End Using
    End Function

    Public Async Function GetMeetingLookupAsync() As Task(Of IEnumerable(Of LookupItem)) Implements IMeetingLookupDataService.GetMeetingLookupAsync
        Using ctx = ContextCreator()
            Return Await ctx.Meetings.AsNoTracking.Select(Function(i) New LookupItem With {.Id = i.Id, .DisplayMember = i.Title}).ToListAsync
        End Using
    End Function

End Class
