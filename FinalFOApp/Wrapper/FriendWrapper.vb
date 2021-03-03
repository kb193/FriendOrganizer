Imports Model

Public Class FriendWrapper
    Inherits ModelWrapper(Of [Friend])

    Public Sub New(model As [Friend])
        MyBase.New(model)
    End Sub

    Public ReadOnly Property Id As Integer
        Get
            Return Model.Id
        End Get
    End Property

    Public Property FirstName As String
        Get
            Return GetValue(Of String)()
        End Get
        Set(value As String)
            SetValue(value)
        End Set
    End Property

    Public Property LastName As String
        Get
            Return GetValue(Of String)()
        End Get
        Set(value As String)
            SetValue(value)
        End Set
    End Property

    Public Property FavouriteLanguageId As Integer?
        Get
            Return GetValue(Of Integer?)()
        End Get
        Set(value As Integer?)
            SetValue(value)
        End Set
    End Property

    Protected Overrides Function ValidateProperty(propertyName As String) As IEnumerable(Of String)
        Return MyBase.ValidateProperty(propertyName)
    End Function
End Class
