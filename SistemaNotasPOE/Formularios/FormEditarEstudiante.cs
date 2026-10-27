using System;
using System.Drawing;
using System.Windows.Forms;
using SistemaNotasPOE.Clases;
using SistemaNotasPOE.Repositorios;

namespace SistemaNotasPOE.Formularios
{
    public class FormEditarEstudiante : Form
    {
        private Label lblNombre = null!, lblApellido = null!, lblCodigo = null!;
        private Label lblLaboratorio = null!, lblParcial = null!, lblAsistencia = null!;
        private TextBox txtNombre = null!, txtApellido = null!, txtCodigo = null!;
        private TextBox txtLaboratorio = null!, txtParcial = null!, txtAsistencia = null!;
        private Button btnGuardar = null!, btnCancelar = null!;

        private readonly EstudianteRepo _repo;
        private readonly Estudiante _estudiante;

        public FormEditarEstudiante(Estudiante estudiante)
        {
            _repo = new EstudianteRepo();
            _estudiante = estudiante;
            InicializarComponentes();
            CargarDatos();
        }

        private void InicializarComponentes()
        {
            this.Text = "Editar Estudiante";
            this.Size = new Size(380, 320);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            lblNombre = new Label { Text = "Nombre:", Location = new Point(20, 20), AutoSize = true };
            txtNombre = new TextBox { Name = "txtNombre", Location = new Point(130, 18), Width = 200 };

            lblApellido = new Label { Text = "Apellido:", Location = new Point(20, 55), AutoSize = true };
            txtApellido = new TextBox { Name = "txtApellido", Location = new Point(130, 52), Width = 200 };

            lblCodigo = new Label { Text = "Código:", Location = new Point(20, 90), AutoSize = true };
            txtCodigo = new TextBox { Name = "txtCodigo", Location = new Point(130, 87), Width = 200 };

            lblLaboratorio = new Label { Text = "Laboratorio:", Location = new Point(20, 125), AutoSize = true };
            txtLaboratorio = new TextBox { Name = "txtLaboratorio", Location = new Point(130, 122), Width = 200 };

            lblParcial = new Label { Text = "Parcial:", Location = new Point(20, 160), AutoSize = true };
            txtParcial = new TextBox { Name = "txtParcial", Location = new Point(130, 157), Width = 200 };

            lblAsistencia = new Label { Text = "Asistencia:", Location = new Point(20, 195), AutoSize = true };
            txtAsistencia = new TextBox { Name = "txtAsistencia", Location = new Point(130, 192), Width = 200 };

            btnGuardar = new Button { Text = "Guardar", Location = new Point(130, 230), Width = 95, Height = 30 };
            btnCancelar = new Button { Text = "Cancelar", Location = new Point(235, 230), Width = 95, Height = 30 };

            txtLaboratorio.KeyPress += ValidarSoloNumerosDecimales;
            txtParcial.KeyPress += ValidarSoloNumerosDecimales;
            txtAsistencia.KeyPress += ValidarSoloNumerosDecimales;

            btnGuardar.Click += BtnGuardar_Click;
            btnCancelar.Click += (s, e) => this.DialogResult = DialogResult.Cancel;

            this.Controls.Add(lblNombre);
            this.Controls.Add(txtNombre);
            this.Controls.Add(lblApellido);
            this.Controls.Add(txtApellido);
            this.Controls.Add(lblCodigo);
            this.Controls.Add(txtCodigo);
            this.Controls.Add(lblLaboratorio);
            this.Controls.Add(txtLaboratorio);
            this.Controls.Add(lblParcial);
            this.Controls.Add(txtParcial);
            this.Controls.Add(lblAsistencia);
            this.Controls.Add(txtAsistencia);
            this.Controls.Add(btnGuardar);
            this.Controls.Add(btnCancelar);
        }

        private void CargarDatos()
        {
            txtNombre.Text = _estudiante.Nombre;
            txtApellido.Text = _estudiante.Apellido;
            txtCodigo.Text = _estudiante.Codigo;
            txtLaboratorio.Text = _estudiante.Laboratorio.ToString("0.00");
            txtParcial.Text = _estudiante.Parcial.ToString("0.00");
            txtAsistencia.Text = _estudiante.Asistencia.ToString("0.00");
        }

        private void ValidarSoloNumerosDecimales(object? sender, KeyPressEventArgs e)
        {
            TextBox textBox = (TextBox)sender!;
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            if ((e.KeyChar == '.') && (textBox.Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void BtnGuardar_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text) ||
                string.IsNullOrWhiteSpace(txtApellido.Text) ||
                string.IsNullOrWhiteSpace(txtCodigo.Text) ||
                string.IsNullOrWhiteSpace(txtLaboratorio.Text) ||
                string.IsNullOrWhiteSpace(txtParcial.Text) ||
                string.IsNullOrWhiteSpace(txtAsistencia.Text))
            {
                MessageBox.Show("Todos los campos son obligatorios.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!decimal.TryParse(txtLaboratorio.Text, out decimal lab) || lab < 0m || lab > 10.0m ||
                !decimal.TryParse(txtParcial.Text, out decimal par) || par < 0m || par > 10.0m ||
                !decimal.TryParse(txtAsistencia.Text, out decimal asi) || asi < 0m || asi > 10.0m)
            {
                MessageBox.Show("Las notas deben estar en un rango de 0.0 a 10.0.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            decimal notaFinal = (lab * 0.5m) + (par * 0.4m) + (asi * 0.1m);

            _estudiante.Nombre = txtNombre.Text.Trim().ToUpper();
            _estudiante.Apellido = txtApellido.Text.Trim().ToUpper();
            _estudiante.Codigo = txtCodigo.Text.Trim().ToUpper();
            _estudiante.Laboratorio = lab;
            _estudiante.Parcial = par;
            _estudiante.Asistencia = asi;
            _estudiante.NotaFinal = Math.Round(notaFinal, 2);

            _repo.Actualizar(_estudiante);
            this.DialogResult = DialogResult.OK;
        }
    }
}