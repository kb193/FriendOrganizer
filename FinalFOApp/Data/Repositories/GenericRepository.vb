Imports System.Data.Entity
Imports DataAccess

Public Class GenericRepository(Of TEntity As Class, TContext As DbContext)
    Implements IGenericRepository(Of TEntity)

    Public Context As FinalDbContext

    Sub New(context As FinalDbContext)
        Me.Context = context
    End Sub

    Public Sub Add(model As TEntity) Implements IGenericRepository(Of TEntity).Add
        Context.Set(Of TEntity).Add(model)
    End Sub

    Public Sub Remove(model As TEntity) Implements IGenericRepository(Of TEntity).Remove
        Context.Set(Of TEntity).Remove(model)
    End Sub

    Public Overridable Async Function GetByIdAsync(id As Integer) As Task(Of TEntity) Implements IGenericRepository(Of TEntity).GetByIdAsync
        Return Await Context.Set(Of TEntity).FindAsync(id)
    End Function

    Public Async Function SaveAsync() As Task Implements IGenericRepository(Of TEntity).SaveAsync
        Await Context.SaveChangesAsync()
    End Function

    Public Function HasChanges() As Boolean Implements IGenericRepository(Of TEntity).HasChanges
        Return Context.ChangeTracker.HasChanges()
    End Function

    Public Async Function GetAllAsync() As Task(Of IEnumerable(Of TEntity)) Implements IGenericRepository(Of TEntity).GetAllAsync
        Return Await Context.Set(Of TEntity).ToListAsync
    End Function

End Class
