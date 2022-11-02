Public Class FormEmpleados

    Public conexion As New Conexion
    Dim validaciones As Validaciones = New Validaciones()

    Private Sub FormArticulos_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        conexion.Conectar()
        conexion.CargarDatosEmpleados()

        dgvLista.DataSource = conexion.MiDataSet
        dgvLista.DataMember = "Empleados"
        editarCols()
    End Sub

    Private Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        txtId.Text = ""
        txtNombre.Text = ""
        txtPswd.Text = ""
        txtSueldo.Text = ""
        txtAdmin.Text = ""
        txtPuesto.Text = ""
    End Sub

    Private Sub btnEditar_Click(sender As Object, e As EventArgs) Handles btnEditar.Click
        Dim miDataRow As DataRow
        Dim i As Integer
        i = dgvLista.CurrentRow.Index
        miDataRow = conexion.MiDataSet.Tables("Empleados").Rows(i)
        miDataRow("idEmpleado") = txtId.Text
        miDataRow("Nombre") = txtNombre.Text
        miDataRow("Contraseña") = txtPswd.Text
        miDataRow("Sueldo") = txtSueldo.Text
        miDataRow("Admin") = txtAdmin.Text
        miDataRow("Puesto") = txtPuesto.Text
        conexion.MiDataAdapter.Update(conexion.MiDataSet, "Empleados")

        conexion.ActualizarDgvEmpleados()
        MsgBox("Info del empleado modificada")
    End Sub

    Private Sub btnBorrar_Click(sender As Object, e As EventArgs) Handles btnBorrar.Click
        Dim miDataRow As DataRow
        Dim i As Integer
        i = dgvLista.CurrentRow.Index
        miDataRow = conexion.MiDataSet.Tables("Empleados").Rows(i)
        miDataRow.Delete()
        Dim TablaBorrados As DataTable
        TablaBorrados = conexion.MiDataSet.Tables("Empleados").GetChanges(DataRowState.Deleted)
        conexion.MiDataAdapter2.Update(TablaBorrados)
        conexion.MiDataSet.Tables("Empleados").AcceptChanges()
        btnCancelar.PerformClick()
        MsgBox("Empleado Eliminado")
    End Sub

    Private Sub btnNuevo_Click(sender As Object, e As EventArgs) Handles btnNuevo.Click
        Dim comprobaciones As Boolean = True
        If validaciones.validarNombre(txtNombre.Text.Trim) = False Then
            MessageBox.Show(validaciones.mensaje, "Informacion")
            txtNombre.Focus()
            comprobaciones = False
        End If

        If validaciones.validarPswd(txtPswd.Text.Trim) = False Then
            MessageBox.Show(validaciones.mensaje, "Informacion")
            txtPswd.Focus()
            comprobaciones = False
        End If

        If validaciones.validarPuesto(txtPuesto.Text.Trim) = False Then
            MessageBox.Show(validaciones.mensaje, "Informacion")
            txtPuesto.Focus()
            comprobaciones = False
        End If

        If comprobaciones = True Then
            If validaciones.validarId(txtId.Text.Trim) = False Then
                MessageBox.Show(validaciones.mensaje, "Informacion")
                txtId.Focus()
                comprobaciones = False
            End If
        End If

        If comprobaciones = True Then
            If validaciones.validarSueldo(txtSueldo.Text.Trim) = False Then
                MessageBox.Show(validaciones.mensaje, "Informacion")
                txtSueldo.Focus()
                comprobaciones = False
            End If
        End If

        If comprobaciones = True Then
            If validaciones.validarAdmin(txtAdmin.Text.Trim) = False Then
                MessageBox.Show(validaciones.mensaje, "Informacion")
                txtAdmin.Focus()
                comprobaciones = False
            End If
        End If

        If comprobaciones = True Then
            Dim miDataRow As DataRow
            miDataRow = conexion.MiDataSet.Tables("Empleados").NewRow
            miDataRow("idEmpleado") = txtId.Text
            miDataRow("Nombre") = txtNombre.Text
            miDataRow("Contraseña") = txtPswd.Text
            miDataRow("Sueldo") = txtSueldo.Text
            miDataRow("Admin") = txtAdmin.Text
            miDataRow("Puesto") = txtPuesto.Text


            conexion.MiDataSet.Tables("Empleados").Rows.Add(miDataRow)
            conexion.MiDataAdapter2.Update(conexion.MiDataSet, "Empleados")

            conexion.ActualizarDgvArticulos()
            MsgBox("Empleado añadido")
        End If

    End Sub

    Private Sub dgvLista_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvLista.CellClick
        txtId.Text = dgvLista.SelectedCells.Item(0).Value
        txtNombre.Text = dgvLista.SelectedCells.Item(1).Value
        txtPswd.Text = dgvLista.SelectedCells.Item(2).Value
        txtSueldo.Text = dgvLista.SelectedCells.Item(3).Value
        txtAdmin.Text = dgvLista.SelectedCells.Item(4).Value
        txtPuesto.Text = dgvLista.SelectedCells.Item(5).Value
    End Sub

    Private Sub Buscar()
        Dim dtTabla As New DataTable
        conexion.MiDataAdapter2.Fill(dtTabla)
        Try
            Dim DataSet As New DataSet
            DataSet.Tables.Add(dtTabla.Copy)
            Dim DataView As New DataView(DataSet.Tables(0))

            DataView.RowFilter = cbBuscar.Text & " like '" & txtBuscar.Text & "%'"

            If DataView.Count <> 0 Then
                dgvLista.DataSource = DataView
            End If

        Catch ex As Exception
            MsgBox("Error")
        End Try
        editarCols()
    End Sub

    Private Sub editarCols()
        dgvLista.Columns(0).Visible = False
    End Sub

    Private Sub txtBuscar_TextChanged(sender As Object, e As EventArgs) Handles txtBuscar.TextChanged
        Buscar()
    End Sub
End Class