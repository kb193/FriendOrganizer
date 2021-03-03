Imports Model

Public Class PhoneNumberWrapper
    Inherits ModelWrapper(Of FriendPhoneNumber)

    Public Sub New(model As FriendPhoneNumber)
        MyBase.New(model)
    End Sub

    Public Property Number As String
        Get
            Return GetValue(Of String)()
        End Get
        Set(value As String)
            SetValue(value)
        End Set
    End Property
End Class
