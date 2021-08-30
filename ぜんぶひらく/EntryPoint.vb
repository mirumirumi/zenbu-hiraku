'これは新規のMainメソッドを作成するために新規作成したクラスファイル

Public Class EntryPoint
    <STAThread()>
    Shared Sub Main()
        'Application.EnableVisualStyles()
        'Application.SetCompatibleTextRenderingDefault(False)

        'If Diagnostics.Process.GetProcessesByName(Diagnostics.Process.GetCurrentProcess.ProcessName).Length > 1 Then
        '    MessageBox.Show("「ぜんぶひらく」は既に実行中です。", "おしらせ", MessageBoxButtons.OK, MessageBoxIcon.Asterisk)
        '    Return
        'End If

        'Mutex名を決める
        Dim mutexName As String = "Zenbuhiraku"
        'Mutexオブジェクトを作成する
        Dim mutex As New System.Threading.Mutex(False, mutexName)
        Dim hasHandle As Boolean = False
        Try
            Try
                'ミューテックスの所有権を要求する
                hasHandle = mutex.WaitOne(0, False)
                '.NET Framework 2.0以降の場合　↓
            Catch ex As System.Threading.AbandonedMutexException
                '別のアプリケーションがミューテックスを解放しないで終了した時
                hasHandle = True
            End Try
            'ミューテックスを得られたか調べる
            If hasHandle = False Then
                '得られなかった場合は、すでに起動していると判断して終了
                MessageBox.Show("「ぜんぶひらく」は既に実行中です。", "おしらせ", MessageBoxButtons.OK, MessageBoxIcon.Asterisk)
                Return
            End If

            'はじめからMainメソッドにあったコードを実行
            Dim formMain As New s()
            If My.Settings.chbIsStartAutoRun = True Then
                openAllApp()
            End If
            Application.Run()

        Finally
            If hasHandle Then
                'ミューテックスを解放する
                mutex.ReleaseMutex()
            End If
            mutex.Close()
        End Try
    End Sub
End Class