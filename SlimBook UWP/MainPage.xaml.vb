﻿' The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

Imports Windows.Phone.UI.Input
Imports Windows.UI.Popups

''' <summary>
''' An empty page that can be used on its own or navigated to within a Frame.
''' </summary>
Public NotInheritable Class MainPage
    Inherits Page

    Public MyWebViewSource As String = "https://touch.facebook.com/home.php"

    Public PrivacyInfo As String = "No personal, or private, information of either you, or your device, is collected by this app." & vbCrLf & "Neither is ANY information transmitted by this app."

    Sub BackPressed(sender As Object, e As BackPressedEventArgs)
        Application.Current.Exit()
    End Sub

    Private Sub Home()
        Dim mwv As Uri 'Contains the source URL for Facebook Touch
        mwv = New Uri(MyWebViewSource & "?sk=h_chr")
        SlimBookUWPWebView.Navigate(New Uri(MyWebViewSource))
    End Sub

    Private Async Sub SlimBookUWPWebView_LoadCompleted(sender As Object, e As NavigationEventArgs) Handles SlimBookUWPWebView.LoadCompleted
        Dim cssToApply As String = ""
        cssToApply += "#header {position: fixed; z-index: 12; top: 0px;} #root {padding-top: 44px;} .item.more {position:fixed; bottom: 0px; text-align: center !important;}"
        Dim h = ApplicationView.GetForCurrentView().VisibleBounds.Height - 44
        Dim density As Single = DisplayInformation.GetForCurrentView().LogicalDpi
        Dim barHeight As Integer = CInt((h / density))
        cssToApply += ".flyout {max-height:" & barHeight & "px; overflow-y:scroll;}"
        cssToApply += "#m_newsfeed_stream article[data-ft*=""\\""ei\\"":\\""""] {display:none !important;}"
        Await SlimBookUWPWebView.InvokeScriptAsync("eval", {"javascript:function addStyleString(str) { var node = document.createElement('style'); node.innerHTML = " & "str; document.body.appendChild(node); } addStyleString('" & cssToApply & "');"})
        If Not SlimBookUWPWebView.CanGoBack Then
            abbBack.Label = "Exit"
        Else
            abbBack.Label = "Back"
        End If
        ProgRing.IsActive = False
    End Sub

    Private Sub MainPage_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        Me.InitializeComponent()
        If localSettings.Values.ContainsKey("fullScreen") Then
            If CBool(localSettings.Values("fullScreen")) Then View.TryEnterFullScreenMode()
        Else View.ExitFullScreenMode()
        End If
        _Setting.Visibility = Visibility.Collapsed
        ProgRing.IsActive = True
    End Sub

    Private Sub abbRefresh_Click(sender As Object, e As RoutedEventArgs) Handles abbRefresh.Click
        ProgRing.IsActive = True
        SlimBookUWPWebView.Refresh()
    End Sub

    Private Sub abbHome_Click(sender As Object, e As RoutedEventArgs) Handles abbHome.Click
        Home()
    End Sub

    Private Sub abbAbout_Click(sender As Object, e As RoutedEventArgs) Handles abbAbout.Click
        PivotSettingsAbout.SelectedIndex = 0
        ShowSettings("About SlimBook UWP")
        _Setting.Visibility = Visibility.Visible
    End Sub

    Public Async Function displayMessageAsync(ByVal title As String, ByVal content As String, ByVal dialogType As String) As Task
        Dim messageDialog = New MessageDialog(content, title)
        If dialogType = "notification" Then
        Else
            messageDialog.Commands.Add(New UICommand("Yes", Nothing))
            messageDialog.Commands.Add(New UICommand("No", Nothing))
            messageDialog.DefaultCommandIndex = 0
        End If

        messageDialog.CancelCommandIndex = 1
        Dim cmdResult = Await messageDialog.ShowAsync()
        If cmdResult.Label = "Yes" Then
            Application.Current.Exit()
        End If
    End Function

    Private Async Sub abbBack_Click(sender As Object, e As RoutedEventArgs) Handles abbBack.Click
        If SlimBookUWPWebView.CanGoBack Then
            SlimBookUWPWebView.GoBack()
        Else
            displayMessageAsync("Quit SlimBook UWP", "Are you sure you want to quit the app?", "")
        End If
    End Sub

    Private Sub ShowSettings(ByVal title As String)
        PivotSettingsAbout.SelectedIndex = 0
        _Setting.Visibility = Visibility.Visible
        txtSETTINGSTitle.Text = title
        SettingsSetup()
    End Sub

    Private Sub SettingsSetup()
        Dim number As PackageVersion = Package.Current.Id.Version
        version.Text = String.Format(" {0}.{1}.{2}" & vbCrLf, number.Major, number.Minor, number.Build)
        privacy.Text = PrivacyInfo
    End Sub

    Private Async Sub hyperLogo_Click(sender As Object, e As RoutedEventArgs) Handles hyperLogo.Click
        Dim logoURL = New Uri("http://www.iconarchive.com/show/outline-icons-by-iconsmind/Book-icon.html")
        Await Windows.System.Launcher.LaunchUriAsync(logoURL)
    End Sub

    Private Async Sub hyperDev_Click(sender As Object, e As RoutedEventArgs) Handles hyperDev.Click
        Dim logoURL = New Uri("https://github.com/CelestialDoom/SlimBook-UWP")
        Await Windows.System.Launcher.LaunchUriAsync(logoURL)
    End Sub

    Private Async Sub abbOpenSource_Click(sender As Object, e As RoutedEventArgs) Handles abbOpenSource.Click
        Dim logoURL = New Uri("https://github.com/CelestialDoom/SlimBook-UWP")
        Await Windows.System.Launcher.LaunchUriAsync(logoURL)
    End Sub

    Private Async Sub abbUp_Click(sender As Object, e As RoutedEventArgs) Handles abbUp.Click
        Dim ScrollToTopString = "var int = setInterval(function(){window.scrollBy(0, -36); if( window.pageYOffset === 0 ) clearInterval(int); }, 0.1);"
        Await SlimBookUWPWebView.InvokeScriptAsync("eval", New String() {ScrollToTopString})
    End Sub

    Private Sub CloseGrid_Click(sender As Object, e As RoutedEventArgs) Handles CloseGrid.Click
        _Setting.Visibility = Visibility.Collapsed
    End Sub

End Class