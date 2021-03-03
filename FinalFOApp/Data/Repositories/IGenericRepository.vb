Public Interface IGenericRepository(Of TEntity As Class)
    Sub Add(model As TEntity)
    Sub Remove(model As TEntity)
    Function GetAllAsync() As Task(Of IEnumerable(Of TEntity))
    Function GetByIdAsync(id As Integer) As Task(Of TEntity)
    Function HasChanges() As Boolean
    Function SaveAsync() As Task
End Interface
