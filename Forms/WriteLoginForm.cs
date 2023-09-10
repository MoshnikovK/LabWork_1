using Game2048.Forms;
using Game2048.Models;
using Game2048.Repositories;
using Game2048.Secure;
using System;
using System.Windows.Forms;

namespace Game2048
{

    public partial class WriteLoginForm : BaseForm
    {
        private bool _isLogin;
        private UserRepository _repository;
        private StartForm _startForm;
        private UserMenuForm _userMenu;
        public int? userId = null;
        public WriteLoginForm(BaseForm lastForm, string connectionString, bool isLogin)
        {
            InitializeComponent();
            _repository = new UserRepository(connectionString);
            _startForm = lastForm as StartForm;
            _isLogin = isLogin;
            if (!isLogin)
            {
                label3.Visible = true;
                txtConfirmPassword.Visible = true;
                btnOperation.Text = "Создать";
            }
            else btnOperation.Text = "Войти";
        }

        private async void btnOperation_Click(object sender, EventArgs e)
        {
            switch (_isLogin)
            {
                case true:
                    if ((!String.IsNullOrEmpty(txtUserName.Text) &&
                        !String.IsNullOrWhiteSpace(txtUserName.Text)) &&
                        (!String.IsNullOrEmpty(txtPassword.Text) &&
                        !String.IsNullOrWhiteSpace(txtPassword.Text)))
                    {                      
                        var user = await _repository.GetByName(txtUserName.Text);
                        if (user != null)
                        {
                            var hash = PasswordHasher.Hash(txtPassword.Text);
                            if (user.Password == hash)
                            {
                                userId = user.Id;
                                _userMenu = new UserMenuForm(_repository.ConnectionString, userId.Value, _startForm);
                                _userMenu.Show();
                                this.Hide();
                            }
                            else
                            {
                                MessageBox.Show("Неверный логин или пароль!",
                                      "Ошибка:",
                                      MessageBoxButtons.OK,
                                      MessageBoxIcon.Error);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Такого пользователя не существует!",
                                "Ошибка:",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                        }
                    }
                    break;
                case false:
                    if ((!String.IsNullOrEmpty(txtUserName.Text) &&
                        !String.IsNullOrWhiteSpace(txtUserName.Text)) &&
                        (!String.IsNullOrEmpty(txtPassword.Text) &&
                        !String.IsNullOrWhiteSpace(txtPassword.Text)) &&
                        (!String.IsNullOrEmpty(txtConfirmPassword.Text)) &&
                        (!String.IsNullOrWhiteSpace(txtConfirmPassword.Text)))
                    {
                        var user = await _repository.GetByName(txtUserName.Text);
                        if (user == null)
                        {
                            if (txtPassword.Text == txtConfirmPassword.Text)
                            {
                                var hash = PasswordHasher.Hash(txtPassword.Text);
                                await _repository.AddAsync(new UserModel { Login = txtUserName.Text, Password = hash });
                                MessageBox.Show("Регистрация успешна!",
                                    "Успешно",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Information);
                                OnClose();
                            }
                            else
                            {
                                MessageBox.Show("Пароли не совпадают, повторите попытку!",
                                    "Неуспешно",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Такой пользователь уже существует!",
                                "Ошибка:",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                        }
                    }
                    break;
            }
        }

        private void WriteLoginForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            OnClose();
        }

        private void OnClose()
        {
            this.Hide();
            _startForm.Show();
        }
    }
}