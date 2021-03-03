Imports MahApps.Metro.Controls

Public Class MainWindow
    Inherits MetroWindow

    Private ViewModel As MainWindowViewModel

    Sub New(viewModel As MainWindowViewModel)
        InitializeComponent()
        Me.ViewModel = viewModel

        Me.DataContext = viewModel
    End Sub

    Public Async Sub MainWindow_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        Await ViewModel.LoadAsync()
    End Sub
End Class
