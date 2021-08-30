Imports System.Security.Permissions

Public Class s

    Protected Overrides ReadOnly Property CreateParams() As System.Windows.Forms.CreateParams
        <SecurityPermission(SecurityAction.Demand, Flags:=SecurityPermissionFlag.UnmanagedCode)>
        Get
            Const CS_NOCLOSE As Integer = &H200
            Dim cp As CreateParams = MyBase.CreateParams
            cp.ClassStyle = cp.ClassStyle Or CS_NOCLOSE
            Return cp
        End Get
    End Property

    Private Sub formMain_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        '設定値が全て初期の組み合わせだった場合、設定ファイルのあるディレクトリをgrepしてぜんぶひらくフォルダ内の設定ファイルを見つけてコピー、古いのは消す、を繰り返せば設定は維持される。ユーザーは意識しない。
        '   →明らかに動作が不自然なので混乱を招きそう、デバッグもデバッグモードとか必要になる、コードの対症療法感がヤバいなどなど…でやめる
        '       →だったらまだ設定保存できる方がいいので次回！！
        'If isDefaultAllSettings() Then


        'End If

        'ファイルパスとディレイとウィンドウ状態と無効化を初期化＆読み取り
        initFileSettings()

        '全体ディレイの数字を初期化＆読み取り
        If My.Settings.nudAllDelayTime <> 0 Then
            nudAllDelayTime.Value = My.Settings.nudAllDelayTime
        End If

        'アプリ起動時の自動実行のチェックボックスを初期化＆読み取り
        If My.Settings.chbIsStartAutoRun = True Then
            chbStartAutoRun.Checked = True
        ElseIf My.Settings.chbIsStartAutoRun = False Then
            chbStartAutoRun.Checked = False
        End If

        'Windowsスタート自動実行のチェックボックスを初期化＆読み取り
        If My.Settings.chbIsWindowsAutoRun = True Then
            chbIsWindowsAutoRun.Checked = True
        ElseIf My.Settings.chbIsWindowsAutoRun = False Then
            chbIsWindowsAutoRun.Checked = False
        End If

        '重複起動ON/OFFのチェックボックスを初期化＆読み取り
        If My.Settings.chbDoubleRun = True Then
            chbDoubleRun.Checked = True
        ElseIf My.Settings.chbDoubleRun = False Then
            chbDoubleRun.Checked = False
        End If
    End Sub

    Private Sub btnSettingSave_Click(sender As Object, e As EventArgs) Handles btnSettingSave.Click
        'My.Settingsで保存しているものは、設定メニューで入力した値がデフォルトで読み込まれる（起動時にその変数は既に有効になっているのを確認）

        '/////////////////////各アプリケーションごとの設定/////////////////////
        saveFileSettings()
        '/////////////////////各アプリケーションごとの設定/////////////////////


        '///////////////全体の実行タイミングを遅らせる///////////////
        My.Settings.nudAllDelayTime = nudAllDelayTime.Value
        My.Settings.Save()
        '///////////////全体の実行タイミングを遅らせる///////////////


        '///////////////実行時に自動で機能も実行///////////////
        If chbStartAutoRun.Checked = True Then
            My.Settings.chbIsStartAutoRun = True
            My.Settings.Save()
        End If

        If chbStartAutoRun.Checked = False Then
            My.Settings.chbIsStartAutoRun = False
            My.Settings.Save()
        End If
        '///////////////実行時に自動で機能も実行///////////////


        '///////////////Windowsスタート時に実行する///////////////
        If chbIsWindowsAutoRun.Checked = True Then
            Dim exePath As String = System.Reflection.Assembly.GetExecutingAssembly().Location
            Dim regkey As Microsoft.Win32.RegistryKey =
                    Microsoft.Win32.Registry.CurrentUser.OpenSubKey("Software\Microsoft\Windows\CurrentVersion\Run", True)
            regkey.SetValue("ぜんぶひらく", exePath)
            regkey.Close()
            My.Settings.chbIsWindowsAutoRun = True
            My.Settings.Save()
        End If

        If chbIsWindowsAutoRun.Checked = False Then
            Dim regkey As Microsoft.Win32.RegistryKey =
                Microsoft.Win32.Registry.CurrentUser.OpenSubKey("Software\Microsoft\Windows\CurrentVersion\Run", True)
            regkey.DeleteValue("ぜんぶひらく", False)
            regkey.Close()
            My.Settings.chbIsWindowsAutoRun = False
            My.Settings.Save()
        End If
        '///////////////Windowsスタート時に実行する///////////////


        '//////////////////////重複起動の有無//////////////////////
        If chbDoubleRun.Checked = True Then
            My.Settings.chbDoubleRun = True
            My.Settings.Save()
        End If

        If chbDoubleRun.Checked = False Then
            My.Settings.chbDoubleRun = False
            My.Settings.Save()
        End If
        '//////////////////////重複起動の有無//////////////////////

        Cursor.Current = Cursors.WaitCursor
        System.Threading.Thread.Sleep(217)
        Cursor.Current = Cursors.Default
        Me.Close()
        Me.Dispose()

    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.Close()
        Me.Dispose()
    End Sub

    Private Sub NotifyIcon1_MouseClick(sender As Object, e As MouseEventArgs) Handles NotifyIcon1.MouseClick
        'Me.Show()
    End Sub

    Private Sub 終了ToolStripMenuItem_Click_1(sender As Object, e As EventArgs) Handles 終了ToolStripMenuItem.Click
        Application.Exit()
    End Sub

    Private Sub 設定ToolStripMenuItem_Click_1(sender As Object, e As EventArgs) Handles 設定ToolStripMenuItem.Click
        If Not Application.OpenForms().OfType(Of s).Any Then
            Dim formMainInstance As New s()
            formMainInstance.ShowDialog(Me)
        End If
    End Sub

    Private Sub ぜんぶひらくを実行ToolStripMenuItem_Click_1(sender As Object, e As EventArgs) Handles ぜんぶひらくを実行ToolStripMenuItem.Click
        Dim r As DialogResult = MessageBox.Show("本当に登録アプリケーション全ての起動を開始してよいですか？", "確認", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)
        If r = DialogResult.OK Then
            openAllApp()
        End If
    End Sub

    Private Sub ぜんぶとじるを実行ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ぜんぶとじるを実行ToolStripMenuItem.Click
        Dim r As DialogResult = MessageBox.Show("本当に全てのアプリケーションを終了してもよいですか？", "確認", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)
        If r = DialogResult.OK Then
            closeAllApp()
        End If
    End Sub

    Private Sub tbxAppPath1_TextChanged(sender As Object, e As EventArgs) Handles tbxAppPath1.TextChanged
        If tbxAppPath1.Text = Nothing Then
            pbxApp1.Image = Nothing
            Exit Sub
        End If
        pbxApp1.Image = searchInstallAppIcon(tbxAppPath1.Text)
    End Sub
    Private Sub tbxAppPath2_TextChanged(sender As Object, e As EventArgs) Handles tbxAppPath2.TextChanged
        If tbxAppPath2.Text = Nothing Then
            pbxApp2.Image = Nothing
            Exit Sub
        End If
        pbxApp2.Image = searchInstallAppIcon(tbxAppPath2.Text)
    End Sub
    Private Sub tbxAppPath3_TextChanged(sender As Object, e As EventArgs) Handles tbxAppPath3.TextChanged
        If tbxAppPath3.Text = Nothing Then
            pbxApp3.Image = Nothing
            Exit Sub
        End If
        pbxApp3.Image = searchInstallAppIcon(tbxAppPath3.Text)
    End Sub
    Private Sub tbxAppPath4_TextChanged(sender As Object, e As EventArgs) Handles tbxAppPath4.TextChanged
        If tbxAppPath4.Text = Nothing Then
            pbxApp4.Image = Nothing
            Exit Sub
        End If
        pbxApp4.Image = searchInstallAppIcon(tbxAppPath4.Text)
    End Sub
    Private Sub tbxAppPath5_TextChanged(sender As Object, e As EventArgs) Handles tbxAppPath5.TextChanged
        If tbxAppPath5.Text = Nothing Then
            pbxApp5.Image = Nothing
            Exit Sub
        End If
        pbxApp5.Image = searchInstallAppIcon(tbxAppPath5.Text)
    End Sub
    Private Sub tbxAppPath6_TextChanged(sender As Object, e As EventArgs) Handles tbxAppPath6.TextChanged
        If tbxAppPath6.Text = Nothing Then
            pbxApp6.Image = Nothing
            Exit Sub
        End If
        pbxApp6.Image = searchInstallAppIcon(tbxAppPath6.Text)
    End Sub
    Private Sub tbxAppPath7_TextChanged(sender As Object, e As EventArgs) Handles tbxAppPath7.TextChanged
        If tbxAppPath7.Text = Nothing Then
            pbxApp7.Image = Nothing
            Exit Sub
        End If
        pbxApp7.Image = searchInstallAppIcon(tbxAppPath7.Text)
    End Sub
    Private Sub tbxAppPath8_TextChanged(sender As Object, e As EventArgs) Handles tbxAppPath8.TextChanged
        If tbxAppPath8.Text = Nothing Then
            pbxApp8.Image = Nothing
            Exit Sub
        End If
        pbxApp8.Image = searchInstallAppIcon(tbxAppPath8.Text)
    End Sub
    Private Sub tbxAppPath9_TextChanged(sender As Object, e As EventArgs) Handles tbxAppPath9.TextChanged
        If tbxAppPath9.Text = Nothing Then
            pbxApp9.Image = Nothing
            Exit Sub
        End If
        pbxApp9.Image = searchInstallAppIcon(tbxAppPath9.Text)
    End Sub
    Private Sub tbxAppPath10_TextChanged(sender As Object, e As EventArgs) Handles tbxAppPath10.TextChanged
        If tbxAppPath10.Text = Nothing Then
            pbxApp10.Image = Nothing
            Exit Sub
        End If
        pbxApp10.Image = searchInstallAppIcon(tbxAppPath10.Text)
    End Sub
    Private Sub tbxAppPath11_TextChanged(sender As Object, e As EventArgs) Handles tbxAppPath11.TextChanged
        If tbxAppPath11.Text = Nothing Then
            pbxApp11.Image = Nothing
            Exit Sub
        End If
        pbxApp11.Image = searchInstallAppIcon(tbxAppPath11.Text)
    End Sub
    Private Sub tbxAppPath12_TextChanged(sender As Object, e As EventArgs) Handles tbxAppPath12.TextChanged
        If tbxAppPath12.Text = Nothing Then
            pbxApp12.Image = Nothing
            Exit Sub
        End If
        pbxApp12.Image = searchInstallAppIcon(tbxAppPath12.Text)
    End Sub
    Private Sub tbxAppPath13_TextChanged(sender As Object, e As EventArgs) Handles tbxAppPath13.TextChanged
        If tbxAppPath13.Text = Nothing Then
            pbxApp13.Image = Nothing
            Exit Sub
        End If
        pbxApp13.Image = searchInstallAppIcon(tbxAppPath13.Text)
    End Sub
    Private Sub tbxAppPath14_TextChanged(sender As Object, e As EventArgs) Handles tbxAppPath14.TextChanged
        If tbxAppPath14.Text = Nothing Then
            pbxApp14.Image = Nothing
            Exit Sub
        End If
        pbxApp14.Image = searchInstallAppIcon(tbxAppPath14.Text)
    End Sub
    Private Sub tbxAppPath15_TextChanged(sender As Object, e As EventArgs) Handles tbxAppPath15.TextChanged
        If tbxAppPath15.Text = Nothing Then
            pbxApp15.Image = Nothing
            Exit Sub
        End If
        pbxApp15.Image = searchInstallAppIcon(tbxAppPath15.Text)
    End Sub
    Private Sub tbxAppPath16_TextChanged(sender As Object, e As EventArgs) Handles tbxAppPath16.TextChanged
        If tbxAppPath16.Text = Nothing Then
            pbxApp16.Image = Nothing
            Exit Sub
        End If
        pbxApp16.Image = searchInstallAppIcon(tbxAppPath16.Text)
    End Sub
    Private Sub tbxAppPath17_TextChanged(sender As Object, e As EventArgs) Handles tbxAppPath17.TextChanged
        If tbxAppPath17.Text = Nothing Then
            pbxApp17.Image = Nothing
            Exit Sub
        End If
        pbxApp17.Image = searchInstallAppIcon(tbxAppPath17.Text)
    End Sub
    Private Sub tbxAppPath18_TextChanged(sender As Object, e As EventArgs) Handles tbxAppPath18.TextChanged
        If tbxAppPath18.Text = Nothing Then
            pbxApp18.Image = Nothing
            Exit Sub
        End If
        pbxApp18.Image = searchInstallAppIcon(tbxAppPath18.Text)
    End Sub
    Private Sub tbxAppPath19_TextChanged(sender As Object, e As EventArgs) Handles tbxAppPath19.TextChanged
        If tbxAppPath19.Text = Nothing Then
            pbxApp19.Image = Nothing
            Exit Sub
        End If
        pbxApp19.Image = searchInstallAppIcon(tbxAppPath19.Text)
    End Sub
    Private Sub tbxAppPath20_TextChanged(sender As Object, e As EventArgs) Handles tbxAppPath20.TextChanged
        If tbxAppPath20.Text = Nothing Then
            pbxApp20.Image = Nothing
            Exit Sub
        End If
        pbxApp20.Image = searchInstallAppIcon(tbxAppPath20.Text)
    End Sub
    Private Sub tbxAppPath21_TextChanged(sender As Object, e As EventArgs) Handles tbxAppPath21.TextChanged
        If tbxAppPath21.Text = Nothing Then
            pbxApp21.Image = Nothing
            Exit Sub
        End If
        pbxApp21.Image = searchInstallAppIcon(tbxAppPath21.Text)
    End Sub
    Private Sub tbxAppPath22_TextChanged(sender As Object, e As EventArgs) Handles tbxAppPath22.TextChanged
        If tbxAppPath22.Text = Nothing Then
            pbxApp22.Image = Nothing
            Exit Sub
        End If
        pbxApp22.Image = searchInstallAppIcon(tbxAppPath22.Text)
    End Sub
    Private Sub tbxAppPath23_TextChanged(sender As Object, e As EventArgs) Handles tbxAppPath23.TextChanged
        If tbxAppPath23.Text = Nothing Then
            pbxApp23.Image = Nothing
            Exit Sub
        End If
        pbxApp23.Image = searchInstallAppIcon(tbxAppPath23.Text)
    End Sub
    Private Sub tbxAppPath24_TextChanged(sender As Object, e As EventArgs) Handles tbxAppPath24.TextChanged
        If tbxAppPath24.Text = Nothing Then
            pbxApp24.Image = Nothing
            Exit Sub
        End If
        pbxApp24.Image = searchInstallAppIcon(tbxAppPath24.Text)
    End Sub
    Private Sub tbxAppPath25_TextChanged(sender As Object, e As EventArgs) Handles tbxAppPath25.TextChanged
        If tbxAppPath25.Text = Nothing Then
            pbxApp25.Image = Nothing
            Exit Sub
        End If
        pbxApp25.Image = searchInstallAppIcon(tbxAppPath25.Text)
    End Sub
    Private Sub tbxAppPath26_TextChanged(sender As Object, e As EventArgs) Handles tbxAppPath26.TextChanged
        If tbxAppPath26.Text = Nothing Then
            pbxApp26.Image = Nothing
            Exit Sub
        End If
        pbxApp26.Image = searchInstallAppIcon(tbxAppPath26.Text)
    End Sub
    Private Sub tbxAppPath27_TextChanged(sender As Object, e As EventArgs) Handles tbxAppPath27.TextChanged
        If tbxAppPath27.Text = Nothing Then
            pbxApp27.Image = Nothing
            Exit Sub
        End If
        pbxApp27.Image = searchInstallAppIcon(tbxAppPath27.Text)
    End Sub
    Private Sub tbxAppPath28_TextChanged(sender As Object, e As EventArgs) Handles tbxAppPath28.TextChanged
        If tbxAppPath28.Text = Nothing Then
            pbxApp28.Image = Nothing
            Exit Sub
        End If
        pbxApp28.Image = searchInstallAppIcon(tbxAppPath28.Text)
    End Sub
    Private Sub tbxAppPath29_TextChanged(sender As Object, e As EventArgs) Handles tbxAppPath29.TextChanged
        If tbxAppPath29.Text = Nothing Then
            pbxApp29.Image = Nothing
            Exit Sub
        End If
        pbxApp29.Image = searchInstallAppIcon(tbxAppPath29.Text)
    End Sub
    Private Sub tbxAppPath30_TextChanged(sender As Object, e As EventArgs) Handles tbxAppPath30.TextChanged
        If tbxAppPath30.Text = Nothing Then
            pbxApp30.Image = Nothing
            Exit Sub
        End If
        pbxApp30.Image = searchInstallAppIcon(tbxAppPath30.Text)
    End Sub
    Private Sub tbxAppPath31_TextChanged(sender As Object, e As EventArgs) Handles tbxAppPath31.TextChanged
        If tbxAppPath31.Text = Nothing Then
            pbxApp31.Image = Nothing
            Exit Sub
        End If
        pbxApp31.Image = searchInstallAppIcon(tbxAppPath31.Text)
    End Sub

    Public Sub initFileSettings()
        If My.Settings.filePath1 <> "" Then
            tbxAppPath1.Text = My.Settings.filePath1
        End If
        If My.Settings.fileDelay1 <> 0 Then
            nudAppDelayTime1.Value = My.Settings.fileDelay1
        End If
        If My.Settings.disabled1 <> True Then  'ここだけデフォルトON
            DisabledCheckBox1.Checked = My.Settings.disabled1
        End If
        If My.Settings.windowState1 <> 0 Then
            cmbWindowState1.SelectedIndex = My.Settings.windowState1
        End If

        If My.Settings.filePath2 <> "" Then
            tbxAppPath2.Text = My.Settings.filePath2
        End If
        If My.Settings.fileDelay2 <> 0 Then
            nudAppDelayTime2.Value = My.Settings.fileDelay2
        End If
        If My.Settings.disabled2 <> False Then
            DisabledCheckBox2.Checked = My.Settings.disabled2
        End If
        If My.Settings.windowState2 <> 0 Then
            cmbWindowState2.SelectedIndex = My.Settings.windowState2
        End If

        If My.Settings.filePath3 <> "" Then
            tbxAppPath3.Text = My.Settings.filePath3
        End If
        If My.Settings.fileDelay3 <> 0 Then
            nudAppDelayTime3.Value = My.Settings.fileDelay3
        End If
        If My.Settings.disabled3 <> False Then
            DisabledCheckBox3.Checked = My.Settings.disabled3
        End If
        If My.Settings.windowState3 <> 0 Then
            cmbWindowState3.SelectedIndex = My.Settings.windowState3
        End If

        If My.Settings.filePath4 <> "" Then
            tbxAppPath4.Text = My.Settings.filePath4
        End If
        If My.Settings.fileDelay4 <> 0 Then
            nudAppDelayTime4.Value = My.Settings.fileDelay4
        End If
        If My.Settings.disabled4 <> False Then
            DisabledCheckBox4.Checked = My.Settings.disabled4
        End If
        If My.Settings.windowState4 <> 0 Then
            cmbWindowState4.SelectedIndex = My.Settings.windowState4
        End If

        If My.Settings.filePath5 <> "" Then
            tbxAppPath5.Text = My.Settings.filePath5
        End If
        If My.Settings.fileDelay5 <> 0 Then
            nudAppDelayTime5.Value = My.Settings.fileDelay5
        End If
        If My.Settings.disabled5 <> False Then
            DisabledCheckBox5.Checked = My.Settings.disabled5
        End If
        If My.Settings.windowState5 <> 0 Then
            cmbWindowState5.SelectedIndex = My.Settings.windowState5
        End If

        If My.Settings.filePath6 <> "" Then
            tbxAppPath6.Text = My.Settings.filePath6
        End If
        If My.Settings.fileDelay6 <> 0 Then
            nudAppDelayTime6.Value = My.Settings.fileDelay6
        End If
        If My.Settings.disabled6 <> False Then
            DisabledCheckBox6.Checked = My.Settings.disabled6
        End If
        If My.Settings.windowState6 <> 0 Then
            cmbWindowState6.SelectedIndex = My.Settings.windowState6
        End If

        If My.Settings.filePath7 <> "" Then
            tbxAppPath7.Text = My.Settings.filePath7
        End If
        If My.Settings.fileDelay7 <> 0 Then
            nudAppDelayTime7.Value = My.Settings.fileDelay7
        End If
        If My.Settings.disabled7 <> False Then
            DisabledCheckBox7.Checked = My.Settings.disabled7
        End If
        If My.Settings.windowState7 <> 0 Then
            cmbWindowState7.SelectedIndex = My.Settings.windowState7
        End If

        If My.Settings.filePath8 <> "" Then
            tbxAppPath8.Text = My.Settings.filePath8
        End If
        If My.Settings.fileDelay8 <> 0 Then
            nudAppDelayTime8.Value = My.Settings.fileDelay8
        End If
        If My.Settings.disabled8 <> False Then
            DisabledCheckBox8.Checked = My.Settings.disabled8
        End If
        If My.Settings.windowState8 <> 0 Then
            cmbWindowState8.SelectedIndex = My.Settings.windowState8
        End If

        If My.Settings.filePath9 <> "" Then
            tbxAppPath9.Text = My.Settings.filePath9
        End If
        If My.Settings.fileDelay9 <> 0 Then
            nudAppDelayTime9.Value = My.Settings.fileDelay9
        End If
        If My.Settings.disabled9 <> False Then
            DisabledCheckBox9.Checked = My.Settings.disabled9
        End If
        If My.Settings.windowState9 <> 0 Then
            cmbWindowState9.SelectedIndex = My.Settings.windowState9
        End If

        If My.Settings.filePath10 <> "" Then
            tbxAppPath10.Text = My.Settings.filePath10
        End If
        If My.Settings.fileDelay10 <> 0 Then
            nudAppDelayTime10.Value = My.Settings.fileDelay10
        End If
        If My.Settings.disabled10 <> False Then
            DisabledCheckBox10.Checked = My.Settings.disabled10
        End If
        If My.Settings.windowState10 <> 0 Then
            cmbWindowState10.SelectedIndex = My.Settings.windowState10
        End If

        If My.Settings.filePath11 <> "" Then
            tbxAppPath11.Text = My.Settings.filePath11
        End If
        If My.Settings.fileDelay11 <> 0 Then
            nudAppDelayTime11.Value = My.Settings.fileDelay11
        End If
        If My.Settings.disabled11 <> False Then
            DisabledCheckBox11.Checked = My.Settings.disabled11
        End If
        If My.Settings.windowState11 <> 0 Then
            cmbWindowState11.SelectedIndex = My.Settings.windowState11
        End If

        If My.Settings.filePath12 <> "" Then
            tbxAppPath12.Text = My.Settings.filePath12
        End If
        If My.Settings.fileDelay12 <> 0 Then
            nudAppDelayTime12.Value = My.Settings.fileDelay12
        End If
        If My.Settings.disabled12 <> False Then
            DisabledCheckBox12.Checked = My.Settings.disabled12
        End If
        If My.Settings.windowState12 <> 0 Then
            cmbWindowState12.SelectedIndex = My.Settings.windowState12
        End If

        If My.Settings.filePath13 <> "" Then
            tbxAppPath13.Text = My.Settings.filePath13
        End If
        If My.Settings.fileDelay13 <> 0 Then
            nudAppDelayTime13.Value = My.Settings.fileDelay13
        End If
        If My.Settings.disabled13 <> False Then
            DisabledCheckBox13.Checked = My.Settings.disabled13
        End If
        If My.Settings.windowState13 <> 0 Then
            cmbWindowState13.SelectedIndex = My.Settings.windowState13
        End If

        If My.Settings.filePath14 <> "" Then
            tbxAppPath14.Text = My.Settings.filePath14
        End If
        If My.Settings.fileDelay14 <> 0 Then
            nudAppDelayTime14.Value = My.Settings.fileDelay14
        End If
        If My.Settings.disabled14 <> False Then
            DisabledCheckBox14.Checked = My.Settings.disabled14
        End If
        If My.Settings.windowState14 <> 0 Then
            cmbWindowState14.SelectedIndex = My.Settings.windowState14
        End If

        If My.Settings.filePath15 <> "" Then
            tbxAppPath15.Text = My.Settings.filePath15
        End If
        If My.Settings.fileDelay15 <> 0 Then
            nudAppDelayTime15.Value = My.Settings.fileDelay15
        End If
        If My.Settings.disabled15 <> False Then
            DisabledCheckBox15.Checked = My.Settings.disabled15
        End If
        If My.Settings.windowState15 <> 0 Then
            cmbWindowState15.SelectedIndex = My.Settings.windowState15
        End If

        If My.Settings.filePath16 <> "" Then
            tbxAppPath16.Text = My.Settings.filePath16
        End If
        If My.Settings.fileDelay16 <> 0 Then
            nudAppDelayTime16.Value = My.Settings.fileDelay16
        End If
        If My.Settings.disabled16 <> False Then
            DisabledCheckBox16.Checked = My.Settings.disabled16
        End If
        If My.Settings.windowState16 <> 0 Then
            cmbWindowState16.SelectedIndex = My.Settings.windowState16
        End If

        If My.Settings.filePath17 <> "" Then
            tbxAppPath17.Text = My.Settings.filePath17
        End If
        If My.Settings.fileDelay17 <> 0 Then
            nudAppDelayTime17.Value = My.Settings.fileDelay17
        End If
        If My.Settings.disabled17 <> False Then
            DisabledCheckBox17.Checked = My.Settings.disabled17
        End If
        If My.Settings.windowState17 <> 0 Then
            cmbWindowState17.SelectedIndex = My.Settings.windowState17
        End If

        If My.Settings.filePath18 <> "" Then
            tbxAppPath18.Text = My.Settings.filePath18
        End If
        If My.Settings.fileDelay18 <> 0 Then
            nudAppDelayTime18.Value = My.Settings.fileDelay18
        End If
        If My.Settings.disabled18 <> False Then
            DisabledCheckBox18.Checked = My.Settings.disabled18
        End If
        If My.Settings.windowState18 <> 0 Then
            cmbWindowState18.SelectedIndex = My.Settings.windowState18
        End If

        If My.Settings.filePath19 <> "" Then
            tbxAppPath19.Text = My.Settings.filePath19
        End If
        If My.Settings.fileDelay19 <> 0 Then
            nudAppDelayTime19.Value = My.Settings.fileDelay19
        End If
        If My.Settings.disabled19 <> False Then
            DisabledCheckBox19.Checked = My.Settings.disabled19
        End If
        If My.Settings.windowState19 <> 0 Then
            cmbWindowState19.SelectedIndex = My.Settings.windowState19
        End If

        If My.Settings.filePath20 <> "" Then
            tbxAppPath20.Text = My.Settings.filePath20
        End If
        If My.Settings.fileDelay20 <> 0 Then
            nudAppDelayTime20.Value = My.Settings.fileDelay20
        End If
        If My.Settings.disabled20 <> False Then
            DisabledCheckBox20.Checked = My.Settings.disabled20
        End If
        If My.Settings.windowState20 <> 0 Then
            cmbWindowState20.SelectedIndex = My.Settings.windowState20
        End If

        If My.Settings.filePath21 <> "" Then
            tbxAppPath21.Text = My.Settings.filePath21
        End If
        If My.Settings.fileDelay21 <> 0 Then
            nudAppDelayTime21.Value = My.Settings.fileDelay21
        End If
        If My.Settings.disabled21 <> False Then
            DisabledCheckBox21.Checked = My.Settings.disabled21
        End If
        If My.Settings.windowState21 <> 0 Then
            cmbWindowState21.SelectedIndex = My.Settings.windowState21
        End If

        If My.Settings.filePath22 <> "" Then
            tbxAppPath22.Text = My.Settings.filePath22
        End If
        If My.Settings.fileDelay22 <> 0 Then
            nudAppDelayTime22.Value = My.Settings.fileDelay22
        End If
        If My.Settings.disabled22 <> False Then
            DisabledCheckBox22.Checked = My.Settings.disabled22
        End If
        If My.Settings.windowState22 <> 0 Then
            cmbWindowState22.SelectedIndex = My.Settings.windowState22
        End If

        If My.Settings.filePath23 <> "" Then
            tbxAppPath23.Text = My.Settings.filePath23
        End If
        If My.Settings.fileDelay23 <> 0 Then
            nudAppDelayTime23.Value = My.Settings.fileDelay23
        End If
        If My.Settings.disabled23 <> False Then
            DisabledCheckBox23.Checked = My.Settings.disabled23
        End If
        If My.Settings.windowState23 <> 0 Then
            cmbWindowState23.SelectedIndex = My.Settings.windowState23
        End If

        If My.Settings.filePath24 <> "" Then
            tbxAppPath24.Text = My.Settings.filePath24
        End If
        If My.Settings.fileDelay24 <> 0 Then
            nudAppDelayTime31.Value = My.Settings.fileDelay24
        End If
        If My.Settings.disabled24 <> False Then
            DisabledCheckBox24.Checked = My.Settings.disabled24
        End If
        If My.Settings.windowState24 <> 0 Then
            cmbWindowState24.SelectedIndex = My.Settings.windowState24
        End If

        If My.Settings.filePath25 <> "" Then
            tbxAppPath25.Text = My.Settings.filePath25
        End If
        If My.Settings.fileDelay25 <> 0 Then
            nudAppDelayTime24.Value = My.Settings.fileDelay25
        End If
        If My.Settings.disabled25 <> False Then
            DisabledCheckBox25.Checked = My.Settings.disabled25
        End If
        If My.Settings.windowState25 <> 0 Then
            cmbWindowState25.SelectedIndex = My.Settings.windowState25
        End If

        If My.Settings.filePath26 <> "" Then
            tbxAppPath26.Text = My.Settings.filePath26
        End If
        If My.Settings.fileDelay26 <> 0 Then
            nudAppDelayTime25.Value = My.Settings.fileDelay26
        End If
        If My.Settings.disabled26 <> False Then
            DisabledCheckBox26.Checked = My.Settings.disabled26
        End If
        If My.Settings.windowState26 <> 0 Then
            cmbWindowState26.SelectedIndex = My.Settings.windowState26
        End If

        If My.Settings.filePath27 <> "" Then
            tbxAppPath27.Text = My.Settings.filePath27
        End If
        If My.Settings.fileDelay27 <> 0 Then
            nudAppDelayTime26.Value = My.Settings.fileDelay27
        End If
        If My.Settings.disabled27 <> False Then
            DisabledCheckBox27.Checked = My.Settings.disabled27
        End If
        If My.Settings.windowState27 <> 0 Then
            cmbWindowState27.SelectedIndex = My.Settings.windowState27
        End If

        If My.Settings.filePath28 <> "" Then
            tbxAppPath28.Text = My.Settings.filePath28
        End If
        If My.Settings.fileDelay28 <> 0 Then
            nudAppDelayTime27.Value = My.Settings.fileDelay28
        End If
        If My.Settings.disabled28 <> False Then
            DisabledCheckBox28.Checked = My.Settings.disabled28
        End If
        If My.Settings.windowState28 <> 0 Then
            cmbWindowState28.SelectedIndex = My.Settings.windowState28
        End If

        If My.Settings.filePath29 <> "" Then
            tbxAppPath29.Text = My.Settings.filePath29
        End If
        If My.Settings.fileDelay29 <> 0 Then
            nudAppDelayTime28.Value = My.Settings.fileDelay29
        End If
        If My.Settings.disabled29 <> False Then
            DisabledCheckBox29.Checked = My.Settings.disabled29
        End If
        If My.Settings.windowState29 <> 0 Then
            cmbWindowState29.SelectedIndex = My.Settings.windowState29
        End If

        If My.Settings.filePath30 <> "" Then
            tbxAppPath30.Text = My.Settings.filePath30
        End If
        If My.Settings.fileDelay30 <> 0 Then
            nudAppDelayTime29.Value = My.Settings.fileDelay30
        End If
        If My.Settings.disabled30 <> False Then
            DisabledCheckBox30.Checked = My.Settings.disabled30
        End If
        If My.Settings.windowState30 <> 0 Then
            cmbWindowState30.SelectedIndex = My.Settings.windowState30
        End If

        If My.Settings.filePath31 <> "" Then
            tbxAppPath31.Text = My.Settings.filePath31
        End If
        If My.Settings.fileDelay31 <> 0 Then
            nudAppDelayTime30.Value = My.Settings.fileDelay31
        End If
        If My.Settings.disabled31 <> False Then
            DisabledCheckBox31.Checked = My.Settings.disabled31
        End If
        If My.Settings.windowState31 <> 0 Then
            cmbWindowState31.SelectedIndex = My.Settings.windowState31
        End If
    End Sub

    Private Sub tbxAppPath1_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles tbxAppPath1.KeyDown
        If e.KeyCode = Keys.A AndAlso e.Control Then
            DirectCast(sender, TextBox).SelectAll()
            e.SuppressKeyPress = True
        End If
    End Sub

    Private Sub tbxAppPath2_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles tbxAppPath2.KeyDown
        If e.KeyCode = Keys.A AndAlso e.Control Then
            DirectCast(sender, TextBox).SelectAll()
            e.SuppressKeyPress = True
        End If
    End Sub
    Private Sub tbxAppPath3_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles tbxAppPath3.KeyDown
        If e.KeyCode = Keys.A AndAlso e.Control Then
            DirectCast(sender, TextBox).SelectAll()
            e.SuppressKeyPress = True
        End If
    End Sub
    Private Sub tbxAppPath4_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles tbxAppPath4.KeyDown
        If e.KeyCode = Keys.A AndAlso e.Control Then
            DirectCast(sender, TextBox).SelectAll()
            e.SuppressKeyPress = True
        End If
    End Sub
    Private Sub tbxAppPath5_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles tbxAppPath5.KeyDown
        If e.KeyCode = Keys.A AndAlso e.Control Then
            DirectCast(sender, TextBox).SelectAll()
            e.SuppressKeyPress = True
        End If
    End Sub
    Private Sub tbxAppPath6_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles tbxAppPath6.KeyDown
        If e.KeyCode = Keys.A AndAlso e.Control Then
            DirectCast(sender, TextBox).SelectAll()
            e.SuppressKeyPress = True
        End If
    End Sub
    Private Sub tbxAppPath7_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles tbxAppPath7.KeyDown
        If e.KeyCode = Keys.A AndAlso e.Control Then
            DirectCast(sender, TextBox).SelectAll()
            e.SuppressKeyPress = True
        End If
    End Sub
    Private Sub tbxAppPath8_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles tbxAppPath8.KeyDown
        If e.KeyCode = Keys.A AndAlso e.Control Then
            DirectCast(sender, TextBox).SelectAll()
            e.SuppressKeyPress = True
        End If
    End Sub
    Private Sub tbxAppPath9_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles tbxAppPath9.KeyDown
        If e.KeyCode = Keys.A AndAlso e.Control Then
            DirectCast(sender, TextBox).SelectAll()
            e.SuppressKeyPress = True
        End If
    End Sub
    Private Sub tbxAppPath10_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles tbxAppPath10.KeyDown
        If e.KeyCode = Keys.A AndAlso e.Control Then
            DirectCast(sender, TextBox).SelectAll()
            e.SuppressKeyPress = True
        End If
    End Sub
    Private Sub tbxAppPath11_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles tbxAppPath11.KeyDown
        If e.KeyCode = Keys.A AndAlso e.Control Then
            DirectCast(sender, TextBox).SelectAll()
            e.SuppressKeyPress = True
        End If
    End Sub
    Private Sub tbxAppPath12_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles tbxAppPath12.KeyDown
        If e.KeyCode = Keys.A AndAlso e.Control Then
            DirectCast(sender, TextBox).SelectAll()
            e.SuppressKeyPress = True
        End If
    End Sub
    Private Sub tbxAppPath13_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles tbxAppPath13.KeyDown
        If e.KeyCode = Keys.A AndAlso e.Control Then
            DirectCast(sender, TextBox).SelectAll()
            e.SuppressKeyPress = True
        End If
    End Sub
    Private Sub tbxAppPath14_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles tbxAppPath14.KeyDown
        If e.KeyCode = Keys.A AndAlso e.Control Then
            DirectCast(sender, TextBox).SelectAll()
            e.SuppressKeyPress = True
        End If
    End Sub
    Private Sub tbxAppPath15_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles tbxAppPath15.KeyDown
        If e.KeyCode = Keys.A AndAlso e.Control Then
            DirectCast(sender, TextBox).SelectAll()
            e.SuppressKeyPress = True
        End If
    End Sub
    Private Sub tbxAppPath16_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles tbxAppPath16.KeyDown
        If e.KeyCode = Keys.A AndAlso e.Control Then
            DirectCast(sender, TextBox).SelectAll()
            e.SuppressKeyPress = True
        End If
    End Sub
    Private Sub tbxAppPath17_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles tbxAppPath17.KeyDown
        If e.KeyCode = Keys.A AndAlso e.Control Then
            DirectCast(sender, TextBox).SelectAll()
            e.SuppressKeyPress = True
        End If
    End Sub
    Private Sub tbxAppPath18_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles tbxAppPath18.KeyDown
        If e.KeyCode = Keys.A AndAlso e.Control Then
            DirectCast(sender, TextBox).SelectAll()
            e.SuppressKeyPress = True
        End If
    End Sub
    Private Sub tbxAppPath19_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles tbxAppPath19.KeyDown
        If e.KeyCode = Keys.A AndAlso e.Control Then
            DirectCast(sender, TextBox).SelectAll()
            e.SuppressKeyPress = True
        End If
    End Sub
    Private Sub tbxAppPath20_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles tbxAppPath20.KeyDown
        If e.KeyCode = Keys.A AndAlso e.Control Then
            DirectCast(sender, TextBox).SelectAll()
            e.SuppressKeyPress = True
        End If
    End Sub
    Private Sub tbxAppPath21_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles tbxAppPath21.KeyDown
        If e.KeyCode = Keys.A AndAlso e.Control Then
            DirectCast(sender, TextBox).SelectAll()
            e.SuppressKeyPress = True
        End If
    End Sub
    Private Sub tbxAppPath22_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles tbxAppPath22.KeyDown
        If e.KeyCode = Keys.A AndAlso e.Control Then
            DirectCast(sender, TextBox).SelectAll()
            e.SuppressKeyPress = True
        End If
    End Sub
    Private Sub tbxAppPath23_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles tbxAppPath23.KeyDown
        If e.KeyCode = Keys.A AndAlso e.Control Then
            DirectCast(sender, TextBox).SelectAll()
            e.SuppressKeyPress = True
        End If
    End Sub
    Private Sub tbxAppPath24_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles tbxAppPath24.KeyDown
        If e.KeyCode = Keys.A AndAlso e.Control Then
            DirectCast(sender, TextBox).SelectAll()
            e.SuppressKeyPress = True
        End If
    End Sub
    Private Sub tbxAppPath25_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles tbxAppPath25.KeyDown
        If e.KeyCode = Keys.A AndAlso e.Control Then
            DirectCast(sender, TextBox).SelectAll()
            e.SuppressKeyPress = True
        End If
    End Sub
    Private Sub tbxAppPath26_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles tbxAppPath26.KeyDown
        If e.KeyCode = Keys.A AndAlso e.Control Then
            DirectCast(sender, TextBox).SelectAll()
            e.SuppressKeyPress = True
        End If
    End Sub
    Private Sub tbxAppPath27_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles tbxAppPath27.KeyDown
        If e.KeyCode = Keys.A AndAlso e.Control Then
            DirectCast(sender, TextBox).SelectAll()
            e.SuppressKeyPress = True
        End If
    End Sub
    Private Sub tbxAppPath28_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles tbxAppPath28.KeyDown
        If e.KeyCode = Keys.A AndAlso e.Control Then
            DirectCast(sender, TextBox).SelectAll()
            e.SuppressKeyPress = True
        End If
    End Sub
    Private Sub tbxAppPath29_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles tbxAppPath29.KeyDown
        If e.KeyCode = Keys.A AndAlso e.Control Then
            DirectCast(sender, TextBox).SelectAll()
            e.SuppressKeyPress = True
        End If
    End Sub
    Private Sub tbxAppPath30_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles tbxAppPath30.KeyDown
        If e.KeyCode = Keys.A AndAlso e.Control Then
            DirectCast(sender, TextBox).SelectAll()
            e.SuppressKeyPress = True
        End If
    End Sub
    Private Sub tbxAppPath31_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles tbxAppPath31.KeyDown
        If e.KeyCode = Keys.A AndAlso e.Control Then
            DirectCast(sender, TextBox).SelectAll()
            e.SuppressKeyPress = True
        End If
    End Sub

    Public Sub saveFileSettings()
        My.Settings.filePath1 = tbxAppPath1.Text
        My.Settings.fileDelay1 = nudAppDelayTime1.Value
        My.Settings.disabled1 = DisabledCheckBox1.Checked
        My.Settings.windowState1 = cmbWindowState1.SelectedIndex
        My.Settings.filePath2 = tbxAppPath2.Text
        My.Settings.fileDelay2 = nudAppDelayTime2.Value
        My.Settings.disabled2 = DisabledCheckBox2.Checked
        My.Settings.windowState2 = cmbWindowState2.SelectedIndex
        My.Settings.filePath3 = tbxAppPath3.Text
        My.Settings.fileDelay3 = nudAppDelayTime3.Value
        My.Settings.disabled3 = DisabledCheckBox3.Checked
        My.Settings.windowState3 = cmbWindowState3.SelectedIndex
        My.Settings.filePath4 = tbxAppPath4.Text
        My.Settings.fileDelay4 = nudAppDelayTime4.Value
        My.Settings.disabled4 = DisabledCheckBox4.Checked
        My.Settings.windowState4 = cmbWindowState4.SelectedIndex
        My.Settings.filePath5 = tbxAppPath5.Text
        My.Settings.fileDelay5 = nudAppDelayTime5.Value
        My.Settings.disabled5 = DisabledCheckBox5.Checked
        My.Settings.windowState5 = cmbWindowState5.SelectedIndex
        My.Settings.filePath6 = tbxAppPath6.Text
        My.Settings.fileDelay6 = nudAppDelayTime6.Value
        My.Settings.disabled6 = DisabledCheckBox6.Checked
        My.Settings.windowState6 = cmbWindowState6.SelectedIndex
        My.Settings.filePath7 = tbxAppPath7.Text
        My.Settings.fileDelay7 = nudAppDelayTime7.Value
        My.Settings.disabled7 = DisabledCheckBox7.Checked
        My.Settings.windowState7 = cmbWindowState7.SelectedIndex
        My.Settings.filePath8 = tbxAppPath8.Text
        My.Settings.fileDelay8 = nudAppDelayTime8.Value
        My.Settings.disabled8 = DisabledCheckBox8.Checked
        My.Settings.windowState8 = cmbWindowState8.SelectedIndex
        My.Settings.filePath9 = tbxAppPath9.Text
        My.Settings.fileDelay9 = nudAppDelayTime9.Value
        My.Settings.disabled9 = DisabledCheckBox9.Checked
        My.Settings.windowState9 = cmbWindowState9.SelectedIndex
        My.Settings.filePath10 = tbxAppPath10.Text
        My.Settings.fileDelay10 = nudAppDelayTime10.Value
        My.Settings.disabled10 = DisabledCheckBox10.Checked
        My.Settings.windowState10 = cmbWindowState10.SelectedIndex
        My.Settings.filePath11 = tbxAppPath11.Text
        My.Settings.fileDelay11 = nudAppDelayTime11.Value
        My.Settings.disabled11 = DisabledCheckBox11.Checked
        My.Settings.windowState11 = cmbWindowState11.SelectedIndex
        My.Settings.filePath12 = tbxAppPath12.Text
        My.Settings.fileDelay12 = nudAppDelayTime12.Value
        My.Settings.disabled12 = DisabledCheckBox12.Checked
        My.Settings.windowState12 = cmbWindowState12.SelectedIndex
        My.Settings.filePath13 = tbxAppPath13.Text
        My.Settings.fileDelay13 = nudAppDelayTime13.Value
        My.Settings.disabled13 = DisabledCheckBox13.Checked
        My.Settings.windowState13 = cmbWindowState13.SelectedIndex
        My.Settings.filePath14 = tbxAppPath14.Text
        My.Settings.fileDelay14 = nudAppDelayTime14.Value
        My.Settings.disabled14 = DisabledCheckBox14.Checked
        My.Settings.windowState14 = cmbWindowState14.SelectedIndex
        My.Settings.filePath15 = tbxAppPath15.Text
        My.Settings.fileDelay15 = nudAppDelayTime15.Value
        My.Settings.disabled15 = DisabledCheckBox15.Checked
        My.Settings.windowState15 = cmbWindowState15.SelectedIndex
        My.Settings.filePath16 = tbxAppPath16.Text
        My.Settings.fileDelay16 = nudAppDelayTime16.Value
        My.Settings.disabled16 = DisabledCheckBox16.Checked
        My.Settings.windowState16 = cmbWindowState16.SelectedIndex
        My.Settings.filePath17 = tbxAppPath17.Text
        My.Settings.fileDelay17 = nudAppDelayTime17.Value
        My.Settings.disabled17 = DisabledCheckBox17.Checked
        My.Settings.windowState17 = cmbWindowState17.SelectedIndex
        My.Settings.filePath18 = tbxAppPath18.Text
        My.Settings.fileDelay18 = nudAppDelayTime18.Value
        My.Settings.disabled18 = DisabledCheckBox18.Checked
        My.Settings.windowState18 = cmbWindowState18.SelectedIndex
        My.Settings.filePath19 = tbxAppPath19.Text
        My.Settings.fileDelay19 = nudAppDelayTime19.Value
        My.Settings.disabled19 = DisabledCheckBox19.Checked
        My.Settings.windowState19 = cmbWindowState19.SelectedIndex
        My.Settings.filePath20 = tbxAppPath20.Text
        My.Settings.fileDelay20 = nudAppDelayTime20.Value
        My.Settings.disabled20 = DisabledCheckBox20.Checked
        My.Settings.windowState20 = cmbWindowState20.SelectedIndex
        My.Settings.filePath21 = tbxAppPath21.Text
        My.Settings.fileDelay21 = nudAppDelayTime21.Value
        My.Settings.disabled21 = DisabledCheckBox21.Checked
        My.Settings.windowState21 = cmbWindowState21.SelectedIndex
        My.Settings.filePath22 = tbxAppPath22.Text
        My.Settings.fileDelay22 = nudAppDelayTime22.Value
        My.Settings.disabled22 = DisabledCheckBox22.Checked
        My.Settings.windowState22 = cmbWindowState22.SelectedIndex
        My.Settings.filePath23 = tbxAppPath23.Text
        My.Settings.fileDelay23 = nudAppDelayTime23.Value
        My.Settings.disabled23 = DisabledCheckBox23.Checked
        My.Settings.windowState23 = cmbWindowState23.SelectedIndex
        My.Settings.filePath24 = tbxAppPath24.Text
        My.Settings.fileDelay24 = nudAppDelayTime31.Value
        My.Settings.disabled24 = DisabledCheckBox24.Checked
        My.Settings.windowState24 = cmbWindowState24.SelectedIndex
        My.Settings.filePath25 = tbxAppPath25.Text
        My.Settings.fileDelay25 = nudAppDelayTime24.Value
        My.Settings.disabled25 = DisabledCheckBox25.Checked
        My.Settings.windowState25 = cmbWindowState25.SelectedIndex
        My.Settings.filePath26 = tbxAppPath26.Text
        My.Settings.fileDelay26 = nudAppDelayTime25.Value
        My.Settings.disabled26 = DisabledCheckBox26.Checked
        My.Settings.windowState26 = cmbWindowState26.SelectedIndex
        My.Settings.filePath27 = tbxAppPath27.Text
        My.Settings.fileDelay27 = nudAppDelayTime26.Value
        My.Settings.disabled27 = DisabledCheckBox27.Checked
        My.Settings.windowState27 = cmbWindowState27.SelectedIndex
        My.Settings.filePath28 = tbxAppPath28.Text
        My.Settings.fileDelay28 = nudAppDelayTime27.Value
        My.Settings.disabled28 = DisabledCheckBox28.Checked
        My.Settings.windowState28 = cmbWindowState28.SelectedIndex
        My.Settings.filePath29 = tbxAppPath29.Text
        My.Settings.fileDelay29 = nudAppDelayTime28.Value
        My.Settings.disabled29 = DisabledCheckBox29.Checked
        My.Settings.windowState29 = cmbWindowState29.SelectedIndex
        My.Settings.filePath30 = tbxAppPath30.Text
        My.Settings.fileDelay30 = nudAppDelayTime29.Value
        My.Settings.disabled30 = DisabledCheckBox30.Checked
        My.Settings.windowState30 = cmbWindowState30.SelectedIndex
        My.Settings.filePath31 = tbxAppPath31.Text
        My.Settings.fileDelay31 = nudAppDelayTime30.Value
        My.Settings.disabled31 = DisabledCheckBox31.Checked
        My.Settings.windowState31 = cmbWindowState31.SelectedIndex
        My.Settings.Save()
    End Sub

    Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        Dim formDonate As New formDonate()
        formDonate.ShowDialog(Me)
    End Sub
End Class
