using DEV0102.DAL;
using DEV0102.Utils;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DEV0102
{
    public partial class cadUsuario : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.ServerVariables["QUERY_STRING"].Contains("NovoUsuario"))
            {
                panelusuariocadastrado.Visible = false;
            }
            else
            {
                if (Session["codigoUsuario"] == null)
                    Response.Redirect("Login.aspx");
                else
                    panelusuariocadastrado.Visible = true;

            }
        

        }

        protected void btnConsultaCEP_Click(object sender, EventArgs e)
        {
            try
            {
                using (var ws = new wsCorreios.AtendeClienteService())
                {
                    var resultado = ws.consultaCEP(txtCEP.Text);

                    txtEndereco.Text = resultado.end;
                    txtBairro.Text = resultado.bairro;
                    txtCidade.Text = resultado.cidade;
                    txtUF.Text = resultado.uf;
                }
            }
            catch (Exception ex)
            {
                ExibirMensagem(ex.Message);
            }
        }

        public void ExibirMensagem(string msg)
        {
            Response.Write("<script>alert('" + msg + "')</script>");
        }

        public void LimparCampos()
        {
            txtBairro.Text = "";
            txtCEP.Text = "";
            txtCidade.Text = "";
            txtEmail.Text = "";
            txtEndereco.Text = "";
            txtNome.Text = "";
            txtSenha.Text = "";
            txtUF.Text = "";
        }

        protected void btnCadastrar_Click(object sender, EventArgs e)
        {
            try
            {
                if (fupFoto.HasFile)
                {
                    string caminhoArquivo = Server.MapPath("/fotoUsuario/");
                    string nomeArquivo = fupFoto.FileName;

                    fupFoto.SaveAs(caminhoArquivo + nomeArquivo);

                    tabUsuario objusuario = new tabUsuario();
                    objusuario.bairro = txtBairro.Text;
                    objusuario.cep = txtCEP.Text;
                    objusuario.cidade = txtCidade.Text;
                    objusuario.email = txtEmail.Text;
                    objusuario.endereco = txtEndereco.Text;
                    objusuario.nome = txtNome.Text;
                    objusuario.senha = txtSenha.Text;
                    objusuario.uf = txtUF.Text;
                    objusuario.nomeFoto = fupFoto.FileName;

                    tabUsuario objValidador = new tabUsuario();
                    usuarioDAL uDal = new usuarioDAL();

                    if (hiddenfildCodigo.Value != "0")
                    {
                        objusuario.codigo = Convert.ToInt32(hiddenfildCodigo.Value);
                        uDal.Editar(objusuario);
                        hiddenfildCodigo.Value = "0";
                        btnCadastrar.Text = "Cadastrar";
                        gridUsuario.DataBind();
                        LimparCampos();
                        ExibirMensagem("Usuário Editado com sucesso!");

                    }
                    else
                    {

                        objValidador = uDal.consultarUsuarioPorEmail(txtEmail.Text);

                        if (objValidador != null)
                        {
                            ExibirMensagem("Usuário já existe no banco de dados!");
                        }
                        else
                        {
                            uDal.cadastrarUsuario(objusuario);
                            gridUsuario.DataBind();

                            ExibirMensagem("Usuário cadastrado com sucesso!");
                            Suporte objsup = new Suporte();
                            string corpoEmail = "Olá " + txtNome.Text + ", bem vindo ao sistema, você já está cadastrado!";
                            objsup.EnviarEmail("Bem vindo ao Sistema Desenvti", txtEmail.Text, corpoEmail);
                            LimparCampos();
                        }
                    }
                }
                else
                {
                    ExibirMensagem("Selecione uma foto para o usuário");
                }

            }
            catch (Exception)
            {
                ExibirMensagem("Erro ao salvar cadastro! Entre em contato com o administrador do sistema.");
            }

        }

        protected void gridUsuario_RowCommand(object sender, GridViewCommandEventArgs e)
        {


        }

        protected void btnAtualizar_Click(object sender, EventArgs e)
        {
            List<tabUsuario> objlstUsuario = new List<tabUsuario>();

            usuarioDAL uDal = new usuarioDAL();
            objlstUsuario = uDal.listartodosusuario();
            foreach (tabUsuario objU in objlstUsuario)
            {

            }
        }

    }
} 
     