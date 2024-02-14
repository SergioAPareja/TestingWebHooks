<%@ Page Title="" Language="C#" MasterPageFile="~/Layout.master" AutoEventWireup="true" CodeFile="Index.aspx.cs" MaintainScrollPositionOnPostback="false" Inherits="_Default" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager runat="server" ID="scriptManager" />
    <asp:Literal ID="ltlToogleMenu" runat="server"></asp:Literal>
    <style>
        .text-wrap {
            width: 15rem;
        }
        @media screen and (max-width: 800px) {
            .sidebar, .sidebar-wrapper {
                display: block !important;
            }
            .content, .main-panel, .card-body, .card, .container-fluid {
                margin: 0;
                padding:0;
            }
        }
    </style>
    <div class="content px-0 mx-0" style="margin-top:0">
        <div id="headerCard" runat="server" class="container-fluid px-0 mx-0">
            <div class="card">
                <div class="card-body">
                    <div class="row h-25 my-3">
                        <div class="col-2">
                            <img src="assets/img/logo.png" class="logo" />
                        </div>
                        <div class="col text-center w-auto">
                            <h2 style="display: inline-block;" class="wizard-title">Programación de Cursos</h2> 
                            <br />
                            <h3 style="display: inline-block;" class="wizard-title">TITLE CARD</h3>
                        </div>
                        <div class="col-2">
                            <a class="btn btn-success" style="">Exit</a>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-3">
                            <div class="form-group">
                                <label class="w-100" for="email">
                                    Subject
                                </label>
                                <div class="d-inline-flex">
                                    <div style="width:60%; margin: 0 20px 0 0">
                                        <asp:TextBox ID="Subject"  AutoPostBack="true" CssClass="form-control" OnTextChanged="sidebarSearch_TextChanged" runat="server"></asp:TextBox>
                                    </div>
                                    <div style="width:35%">
                                    <asp:DropDownList ID="SubjectSorting" AutoPostBack="true" CssClass="form-control" runat="server" OnTextChanged="gvResultados_Sorting">
                                        <asp:ListItem Value="ASC">Ascendente</asp:ListItem>
                                        <asp:ListItem Value="0">Ordenar</asp:ListItem>
                                        <asp:ListItem Value="DESC">Descendente</asp:ListItem>
                                    </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-3">
                            <div class="form-group">
                                <label for="email" class="w-100">
                                    Titulo
                                </label>
                                <div class="d-inline-flex">
                                    <div style="width:60%; margin: 0 20px 0 0">
                                        <asp:TextBox ID="Titulo"  AutoPostBack="true" CssClass="form-control" runat="server" OnTextChanged="sidebarSearch_TextChanged"></asp:TextBox>
                                    </div>
                                    <div style="width:35%">
                                        <asp:DropDownList ID="TituloSorting" AutoPostBack="true" CssClass="form-control" runat="server" OnTextChanged="gvResultados_Sorting">
                                            <asp:ListItem Value="0">Ordenar</asp:ListItem>
                                            <asp:ListItem Value="ASC">Ascendente</asp:ListItem>
                                            <asp:ListItem Value="DESC">Descendente</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-3">
                            <div class="form-group">
                                <label for="email" class="w-100">
                                    College
                                </label>
                                <div class="d-inline-flex">
                                    <div style="width:60%; margin: 0 20px 0 0">
                                        <asp:TextBox ID="College" CssClass="form-control" runat="server" OnTextChanged="sidebarSearch_TextChanged"></asp:TextBox>
                                    </div>
                                    <div style="width:35%">
                                        <asp:DropDownList ID="CollegeSorting" AutoPostBack="true" CssClass="form-control" runat="server" OnTextChanged="gvResultados_Sorting">
                                            <asp:ListItem Value="0">Ordenar</asp:ListItem>
                                            <asp:ListItem Value="ASC">Ascendente</asp:ListItem>
                                            <asp:ListItem Value="DESC">Descendente</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-3">
                            <div class="form-group">
                                <label for="email" class="w-100">
                                    Sesión
                                </label>
                                <div class="d-inline-flex">
                                    <div style="width:60%; margin: 0 20px 0 0">
                                        <asp:TextBox ID="Sesión" AutoPostBack="true" CssClass="form-control" runat="server" OnTextChanged="sidebarSearch_TextChanged"></asp:TextBox>
                                    </div>
                                    <div style="width:35%">
                                        <asp:DropDownList ID="SesiónSorting" AutoPostBack="true" CssClass="form-control" runat="server" OnTextChanged="gvResultados_Sorting">
                                            <asp:ListItem Value="0">Ordenar</asp:ListItem>
                                            <asp:ListItem Value="ASC">Ascendente</asp:ListItem>
                                            <asp:ListItem Value="DESC">Descendente</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-3">
                            <div class="form-group">
                                <label for="email" class="w-100">
                                    Salón
                                </label>
                                <div class="d-inline-flex">
                                    <div style="width:60%; margin: 0 20px 0 0">
                                        <asp:TextBox ID="Salón" CssClass="form-control" runat="server" OnTextChanged="sidebarSearch_TextChanged"></asp:TextBox>
                                    </div>
                                    <div style="width:35%">
                                        <asp:DropDownList ID="SalónSorting" AutoPostBack="true" CssClass="form-control" runat="server" OnTextChanged="gvResultados_Sorting">
                                            <asp:ListItem Value="0">Ordenar</asp:ListItem>
                                            <asp:ListItem Value="ASC">Ascendente</asp:ListItem>
                                            <asp:ListItem Value="DESC">Descendente</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-3">
                            <div class="form-group">
                                <label for="email" class="w-100">
                                    CRN
                                </label>
                                <div class="d-inline-flex">
                                    <div style="width:60%; margin: 0 20px 0 0">
                                        <asp:TextBox ID="CRN" CssClass="form-control" runat="server" OnTextChanged="sidebarSearch_TextChanged"></asp:TextBox>
                                    </div>
                                    <div style="width:35%">
                                        <asp:DropDownList ID="CRNSorting" AutoPostBack="true" CssClass="form-control" runat="server" OnTextChanged="gvResultados_Sorting">
                                            <asp:ListItem Value="0">Ordenar</asp:ListItem>
                                            <asp:ListItem Value="ASC">Ascendente</asp:ListItem>
                                            <asp:ListItem Value="DESC">Descendente</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-3">
                            <div class="form-group">
                                <label for="email" class="w-100">
                                    Créditos
                                </label>
                                <div class="d-inline-flex">
                                    <div style="width:60%; margin: 0 20px 0 0">
                                        <asp:TextBox ID="Créditos" CssClass="form-control" runat="server" OnTextChanged="sidebarSearch_TextChanged"></asp:TextBox>
                                    </div>
                                    <div style="width:35%">
                                        <asp:DropDownList ID="CréditosSorting" AutoPostBack="true" CssClass="form-control" runat="server" OnTextChanged="gvResultados_Sorting">
                                            <asp:ListItem Value="0">Ordenar</asp:ListItem>
                                            <asp:ListItem Value="ASC">Ascendente</asp:ListItem>
                                            <asp:ListItem Value="DESC">Descendente</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-3">
                            <div class="form-group">
                                <label for="email" class="w-100">
                                    Hora de Entrada
                                </label>
                                <div class="d-inline-flex">
                                    <div style="width:60%; margin: 0 20px 0 0">
                                        <asp:TextBox ID="Entrada" CssClass="form-control" runat="server" OnTextChanged="sidebarSearch_TextChanged"></asp:TextBox>
                                    </div>
                                    <div style="width:35%">
                                        <asp:DropDownList ID="EntradaSorting" AutoPostBack="true" CssClass="form-control" runat="server" OnTextChanged="gvResultados_Sorting">
                                            <asp:ListItem Value="0">Ordenar</asp:ListItem>
                                            <asp:ListItem Value="ASC">Ascendente</asp:ListItem>
                                            <asp:ListItem Value="DESC">Descendente</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-3">
                            <div class="form-group">
                                <label for="email" class="w-100">
                                    Hora de Salida
                                </label>
                                <div class="d-inline-flex">
                                    <div style="width:60%; margin: 0 20px 0 0">
                                        <asp:TextBox ID="Salida" CssClass="form-control" runat="server" OnTextChanged="sidebarSearch_TextChanged"></asp:TextBox>
                                    </div>
                                    <div style="width:35%">
                                        <asp:DropDownList ID="SalidaSorting" AutoPostBack="true" CssClass="form-control" runat="server" OnTextChanged="gvResultados_Sorting">
                                            <asp:ListItem Value="0">Ordenar</asp:ListItem>
                                            <asp:ListItem Value="ASC">Ascendente</asp:ListItem>
                                            <asp:ListItem Value="DESC">Descendente</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-3">
                            <div class="form-group">
                                <label for="email" class="w-100">
                                    Tipo de Curso
                                </label>
                                <div class="d-inline-flex">
                                    <div style="width:60%; margin: 0 20px 0 0">
                                        <asp:TextBox ID="Schedule" CssClass="form-control" runat="server" OnTextChanged="sidebarSearch_TextChanged"></asp:TextBox>
                                    </div>
                                    <div style="width:35%">
                                        <asp:DropDownList ID="ScheduleSorting" AutoPostBack="true" CssClass="form-control" runat="server" OnTextChanged="gvResultados_Sorting">
                                            <asp:ListItem Value="0">Ordenar</asp:ListItem>
                                            <asp:ListItem Value="ASC">Ascendente</asp:ListItem>
                                            <asp:ListItem Value="DESC">Descendente</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-3">
                            <div class="form-group">
                                <label for="email" class="w-100">
                                    PTRM
                                </label>
                                <div class="d-inline-flex">
                                    <div style="width:60%; margin: 0 20px 0 0">
                                        <asp:TextBox ID="PTRM" CssClass="form-control" runat="server" OnTextChanged="sidebarSearch_TextChanged"></asp:TextBox>
                                    </div>
                                    <div style="width:35%">
                                        <asp:DropDownList ID="PTRMSorting" AutoPostBack="true" CssClass="form-control" runat="server" OnTextChanged="gvResultados_Sorting">
                                            <asp:ListItem Value="0">Ordenar</asp:ListItem>
                                            <asp:ListItem Value="ASC">Ascendente</asp:ListItem>
                                            <asp:ListItem Value="DESC">Descendente</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-3">
                            <div class="form-group">
                                <label for="email" class="w-100">
                                    Nivel
                                </label>
                                <div class="d-inline-flex">
                                    <div style="width:60%; margin: 0 20px 0 0">
                                        <asp:TextBox ID="Nivel" CssClass="form-control" runat="server" OnTextChanged="sidebarSearch_TextChanged"></asp:TextBox>
                                    </div>
                                    <div style="width:35%">
                                        <asp:DropDownList ID="NivelSorting" AutoPostBack="true" CssClass="form-control" runat="server" OnTextChanged="gvResultados_Sorting">
                                            <asp:ListItem Value="0">Ordenar</asp:ListItem>
                                            <asp:ListItem Value="ASC">Ascendente</asp:ListItem>
                                            <asp:ListItem Value="DESC">Descendente</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-3" runat="server">
                            <div class="form-group">
                               <div class="d-inline-flex">
                                   <div>
                                        <label for="email" class="w-100">
                                            Do
                                        </label>
                                        <div class="d-inline-flex">
                                            <asp:CheckBox runat="server" ID="dayDo" AutoPostBack="true" OnCheckedChanged="sidebarSearch_TextChanged"/>
                                        </div>
                                    </div>
                                    <div>
                                        <label for="email" class="w-100">
                                            Lu
                                        </label>
                                        <div class="d-inline-flex">
                                            <asp:CheckBox runat="server" ID="dayLu" AutoPostBack="true" OnCheckedChanged="sidebarSearch_TextChanged"/>
                                        </div>
                                    </div>
                                   <div>
                                        <label for="email" class="w-100">
                                            Ma
                                        </label>
                                        <div class="d-inline-flex">
                                            <asp:CheckBox runat="server" ID="dayMa" AutoPostBack="true" OnCheckedChanged="sidebarSearch_TextChanged"/>
                                        </div>
                                    </div>
                                   <div>
                                        <label for="email" class="w-100">
                                            Mi
                                        </label>
                                        <div class="d-inline-flex">
                                            <asp:CheckBox runat="server" ID="dayMi" AutoPostBack="true" OnCheckedChanged="sidebarSearch_TextChanged"/>
                                        </div>
                                    </div>
                                   <div>
                                        <label for="email" class="w-100">
                                            Ju
                                        </label>
                                        <div class="d-inline-flex">
                                            <asp:CheckBox runat="server" ID="dayJu" AutoPostBack="true" OnCheckedChanged="sidebarSearch_TextChanged"/>
                                        </div>
                                    </div>
                                   <div>
                                        <label for="email" class="w-100">
                                            Vi
                                        </label>
                                        <div class="d-inline-flex">
                                            <asp:CheckBox runat="server" ID="dayVi" AutoPostBack="true" OnCheckedChanged="sidebarSearch_TextChanged"/>
                                        </div>
                                    </div>
                                   <div>
                                        <label for="email" class="w-100">
                                            Sa
                                        </label>
                                        <div class="d-inline-flex">
                                            <asp:CheckBox runat="server" ID="daySa" AutoPostBack="true" OnCheckedChanged="sidebarSearch_TextChanged"/>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                        <div class="row">
                            <div class="d-flex flex-column justify-content-center" style="align-items:end; float:right;">
                                <div>
            	                    <a href="index.aspx" class="btn btn-round"><i class="fas fa-sync-alt"></i></a>
            	                    <asp:LinkButton ID="btnBuscarUsuario" CssClass="btn btn-success btn-round btnLoad" runat="server"><i class="fas fa-search"></i></asp:LinkButton>
                                    <asp:LinkButton ID="btnPDF" CssClass="btn btn-success btn-round btnLoad" OnClick="btnPDF_Click" runat="server"><i class="fas fa-file"></i></asp:LinkButton>
                                </div>
                            </div>
                        </div>
                        <div class="text-right">
                            <small id="dateHeader" runat="server" style="text-align:right"></small>
                        </div>
                </div>
            </div>
        </div>
    </div>
    <!--Body Content -->
    <div class="content px-0 mx-0" style="margin-top:0">
        <div class="container-fluid px-0 mx-0">
            <div class="card">
                <div class="card-body">
                    <div class="row">
                        <div class="table-responsive text-center">
                            <asp:GridView ID="gvResultados" CssClass="table table-hover" runat="server" AutoGenerateColumns="false" EmptyDataText="No hay cursos."
                                BorderStyle="None" BorderWidth="0" AllowSorting="true" OnSorting="gvResultados_Sorting" AllowFilteringByColumn="True">
                                <FooterStyle CssClass="footer-grid"/>
                                <Columns>
                                    <asp:TemplateField SortExpression="SUBJ_CRSE">
                                        <HeaderTemplate>
                                            <label style="font-size:xx-small">Subject</label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <div style="font-size:xx-small"><%#DataBinder.Eval(Container.DataItem,"SUBJ_CRSE")%></div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="TITLE">
                                        <HeaderTemplate>
                                            <label style="font-size:xx-small">Titulo</label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <div style="font-size:xx-small"><%#DataBinder.Eval(Container.DataItem,"TITLE")%></div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="COLL_DESC">
                                        <HeaderTemplate>
                                            <label style="font-size:xx-small">College</label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <div style="font-size:xx-small"><%#DataBinder.Eval(Container.DataItem,"COLL_DESC")%></div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="SESS_DESC">
                                        <HeaderTemplate>
                                            <label style="font-size:xx-small">Session</label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <div style="font-size:xx-small"><%#DataBinder.Eval(Container.DataItem,"SESS_DESC")%></div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="BUILDING">
                                        <HeaderTemplate>
                                            <label style="font-size:xx-small">Salón</label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <div style="font-size:xx-small"><%#DataBinder.Eval(Container.DataItem,"BUILDING") + " " + DataBinder.Eval(Container.DataItem,"ROOM")%></div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="CRN">
                                        <HeaderTemplate>
                                            <label style="font-size:xx-small">CRN</label>                                                 
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <div style="font-size:xx-small"><%#DataBinder.Eval(Container.DataItem,"CRN")%></div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="CREDITS">
                                        <HeaderTemplate>
                                            <label style="font-size:xx-small">Créditos</label>                                                  
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <div style="font-size:xx-small"><%#DataBinder.Eval(Container.DataItem,"CREDITS")%></div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="HR_ENTRADA">
                                        <HeaderTemplate>
                                            <label style="font-size:xx-small">Hora de Entrada</label>       
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <div style="font-size:xx-small"><%#DataBinder.Eval(Container.DataItem,"HR_ENTRADA")%></div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="HR_SALIDA">
                                        <HeaderTemplate>
                                            <label style="font-size:xx-small">Hora de Salida</label>                                             
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <div style="font-size:xx-small"><%#DataBinder.Eval(Container.DataItem,"HR_SALIDA")%></div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="DO">
                                        <HeaderTemplate>
                                            <label style="font-size:xx-small">Do</label>                                                   
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <div style="font-size:xx-small"><%#DataBinder.Eval(Container.DataItem,"SUN")%></div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="LU">
                                        <HeaderTemplate>
                                            <label style="font-size:xx-small">Lu</label>                                            
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <div style="font-size:xx-small"><%#DataBinder.Eval(Container.DataItem,"MON")%></div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="MA">
                                        <HeaderTemplate>
                                            <label style="font-size:xx-small">Ma</label>                                                   
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <div style="font-size:xx-small"><%#DataBinder.Eval(Container.DataItem,"TUE")%></div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="MI">
                                        <HeaderTemplate>
                                            <label style="font-size:xx-small">Mi</label>                                                
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <div style="font-size:xx-small"><%#DataBinder.Eval(Container.DataItem,"WED")%></div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="JU">
                                        <HeaderTemplate>
                                            <label style="font-size:xx-small">Ju</label>                                                  
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <div style="font-size:xx-small"><%#DataBinder.Eval(Container.DataItem,"THU")%></div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                     <asp:TemplateField SortExpression="VI">
                                        <HeaderTemplate>
                                            <label style="font-size:xx-small">Vi</label>                                                
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <div style="font-size:xx-small"><%#DataBinder.Eval(Container.DataItem,"FRi")%></div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                     <asp:TemplateField SortExpression="SA">
                                        <HeaderTemplate>
                                            <label style="font-size:xx-small">Sa</label>                                                 
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <div style="font-size:xx-small"><%#DataBinder.Eval(Container.DataItem,"SAT")%></div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="SCHD">
                                        <HeaderTemplate>
                                            <label style="font-size:xx-small">Tipo de Curso</label>                                                   
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <div style="font-size:xx-small"><%#DataBinder.Eval(Container.DataItem,"SCHD").ToString().Replace(" ","-")%></div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="PTRM">
                                        <HeaderTemplate>
                                            <label style="font-size:xx-small">PT</label>                                                 
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <div style="font-size:xx-small"><%#DataBinder.Eval(Container.DataItem,"PTRM_DESC")%></div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="NIVEL">
                                        <HeaderTemplate>
                                            <label style="font-size:xx-small">Nivel</label>                                      
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <div style="font-size:xx-small"><%#DataBinder.Eval(Container.DataItem,"NIVEL")%></div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                        </Columns>
                                <RowStyle CssClass="table-row" />
                                <SelectedRowStyle CssClass="gvSelectedRow" Font-Bold="True" ForeColor="#333333" />
                                <pagerstyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center"></pagerstyle>    
                            </asp:GridView>
                        </div>
                    </div>
                </div>
                <asp:LinkButton type="button" class="btn btn-danger btn-floating btn-lg" id="btnTop" runat="server" OnClick="gotoTop" style="position: fixed; right: 40px; bottom: 40px; z-index:9999999999"><i class="fas fa-arrow-up"></i></asp:LinkButton>
            </div>
        </div>
    </div>
</asp:Content>