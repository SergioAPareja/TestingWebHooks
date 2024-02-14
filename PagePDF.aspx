<%@ Page Title="" Language="C#" MasterPageFile="~/Layout.master" AutoEventWireup="true" CodeFile="PagePDF.aspx.cs" MaintainScrollPositionOnPostback="true" Inherits="_Default" %>

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
                padding: 0;
            }
        }
    </style>
    <div class="content px-0 mx-0" style="margin-top: 0">
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
                            <br />
                            <h6 style="display: inline-block;" class="wizard-title">Filtro</h6>
                        </div>
                    </div>
                    <div class="row">
                        <div class="table-responsive text-center">
                            <asp:GridView ID="filterTable" CssClass="table table-hover" runat="server" AutoGenerateColumns="false" EmptyDataText=""
                                BorderStyle="None" BorderWidth="0" AllowSorting="true" AllowFilteringByColumn="True">
                                <FooterStyle CssClass="footer-grid" />
                                <Columns>
                                    <asp:TemplateField SortExpression="SUBJ_CRSE">
                                        <HeaderTemplate>
                                            <label style="font-size: xx-small">Subject</label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <div style="font-size: xx-small"><%#DataBinder.Eval(Container.DataItem,"SUBJ_CRSE")%></div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="TITLE">
                                        <HeaderTemplate>
                                            <label style="font-size: xx-small">Titulo</label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <div style="font-size: xx-small"><%#DataBinder.Eval(Container.DataItem,"TITLE")%></div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="COLL_DESC">
                                        <HeaderTemplate>
                                            <label style="font-size: xx-small">College</label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <div style="font-size: xx-small"><%#DataBinder.Eval(Container.DataItem,"COLL_DESC")%></div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="SESS">
                                        <HeaderTemplate>
                                            <label style="font-size: xx-small">Session</label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <div style="font-size: xx-small"><%#DataBinder.Eval(Container.DataItem,"SESS_DESC")%></div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="BUILDING">
                                        <HeaderTemplate>
                                            <label style="font-size: xx-small">Salón</label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <div style="font-size: xx-small"><%#DataBinder.Eval(Container.DataItem,"BUILDING")%></div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="CRN">
                                        <HeaderTemplate>
                                            <label style="font-size: xx-small">CRN</label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <div style="font-size: xx-small"><%#DataBinder.Eval(Container.DataItem,"CRN")%></div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="CREDITS">
                                        <HeaderTemplate>
                                            <label style="font-size: xx-small">Créditos</label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <div style="font-size: xx-small"><%#DataBinder.Eval(Container.DataItem,"CREDITS")%></div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="HR_ENTRADA">
                                        <HeaderTemplate>
                                            <label style="font-size: xx-small">Hora de Entrada</label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <div style="font-size: xx-small"><%#DataBinder.Eval(Container.DataItem,"HR_ENTRADA")%></div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="HR_SALIDA">
                                        <HeaderTemplate>
                                            <label style="font-size: xx-small">Hora de Salida</label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <div style="font-size: xx-small"><%#DataBinder.Eval(Container.DataItem,"HR_SALIDA")%></div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="DO">
                                        <HeaderTemplate>
                                            <label style="font-size: xx-small">Do</label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <div style="font-size: xx-small"><%#DataBinder.Eval(Container.DataItem,"SUN")%></div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="LU">
                                        <HeaderTemplate>
                                            <label style="font-size: xx-small">Lu</label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <div style="font-size: xx-small"><%#DataBinder.Eval(Container.DataItem,"MON")%></div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="MA">
                                        <HeaderTemplate>
                                            <label style="font-size: xx-small">Ma</label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <div style="font-size: xx-small"><%#DataBinder.Eval(Container.DataItem,"TUE")%></div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="MI">
                                        <HeaderTemplate>
                                            <label style="font-size: xx-small">Mi</label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <div style="font-size: xx-small"><%#DataBinder.Eval(Container.DataItem,"WED")%></div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="JU">
                                        <HeaderTemplate>
                                            <label style="font-size: xx-small">Ju</label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <div style="font-size: xx-small"><%#DataBinder.Eval(Container.DataItem,"THU")%></div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="VI">
                                        <HeaderTemplate>
                                            <label style="font-size: xx-small">Vi</label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <div style="font-size: xx-small"><%#DataBinder.Eval(Container.DataItem,"FRi")%></div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="SA">
                                        <HeaderTemplate>
                                            <label style="font-size: xx-small">Sa</label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <div style="font-size: xx-small"><%#DataBinder.Eval(Container.DataItem,"SAT")%></div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="SCHD">
                                        <HeaderTemplate>
                                            <label style="font-size: xx-small">Schedule</label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <div style="font-size: xx-small"><%#DataBinder.Eval(Container.DataItem,"SCHD")%></div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="PTRM">
                                        <HeaderTemplate>
                                            <label style="font-size: xx-small">PTRM</label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <div style="font-size: xx-small"><%#DataBinder.Eval(Container.DataItem,"SCHD")%></div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="NIVEL">
                                        <HeaderTemplate>
                                            <label style="font-size: xx-small">NIVEL</label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <div style="font-size: xx-small"><%#DataBinder.Eval(Container.DataItem,"NIVEL")%></div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <RowStyle CssClass="table-row" />
                                <SelectedRowStyle CssClass="gvSelectedRow" Font-Bold="True" ForeColor="#333333" />
                                <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center"></PagerStyle>
                            </asp:GridView>
                        </div>
                    </div>
                    <div class="text-right">
                        <small id="dateHeader" runat="server" style="text-align: right">yaunch</small>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!--Body Content -->
    <div class="content px-0 mx-0" style="margin-top: 0">
        <div class="container-fluid px-0 mx-0">
            <div class="card">
                <div class="card-body">
                    <div class="row">
                        <div class="table-responsive text-center">
                            <asp:GridView ID="gvResultados" CssClass="table table-hover" runat="server" AutoGenerateColumns="false" EmptyDataText="No hay datos."
                                BorderStyle="None" BorderWidth="0" AllowSorting="true" AllowFilteringByColumn="True">
                                <FooterStyle CssClass="footer-grid" />
                                <Columns>
                                    <asp:TemplateField SortExpression="SUBJ_CRSE">
                                        <HeaderTemplate>
                                            <label style="font-size: xx-small">Subject</label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <div style="font-size: xx-small"><%#DataBinder.Eval(Container.DataItem,"SUBJ_CRSE")%></div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="TITLE">
                                        <HeaderTemplate>
                                            <label style="font-size: xx-small">Titulo</label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <div style="font-size: xx-small"><%#DataBinder.Eval(Container.DataItem,"TITLE")%></div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="COLL_DESC">
                                        <HeaderTemplate>
                                            <label style="font-size: xx-small">College</label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <div style="font-size: xx-small"><%#DataBinder.Eval(Container.DataItem,"COLL_DESC")%></div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="SESS">
                                        <HeaderTemplate>
                                            <label style="font-size: xx-small">Session</label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <div style="font-size: xx-small"><%#DataBinder.Eval(Container.DataItem,"SESS_DESC")%></div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="BUILDING">
                                        <HeaderTemplate>
                                            <label style="font-size: xx-small">Salón</label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <div style="font-size: xx-small"><%#DataBinder.Eval(Container.DataItem,"BUILDING") + " " + DataBinder.Eval(Container.DataItem,"ROOM")%></div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="CRN">
                                        <HeaderTemplate>
                                            <label style="font-size: xx-small">CRN</label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <div style="font-size: xx-small"><%#DataBinder.Eval(Container.DataItem,"CRN")%></div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="CREDITS">
                                        <HeaderTemplate>
                                            <label style="font-size: xx-small">Créditos</label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <div style="font-size: xx-small"><%#DataBinder.Eval(Container.DataItem,"CREDITS")%></div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="HR_ENTRADA">
                                        <HeaderTemplate>
                                            <label style="font-size: xx-small">Hora de Entrada</label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <div style="font-size: xx-small"><%#DataBinder.Eval(Container.DataItem,"HR_ENTRADA")%></div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="HR_SALIDA">
                                        <HeaderTemplate>
                                            <label style="font-size: xx-small">Hora de Salida</label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <div style="font-size: xx-small"><%#DataBinder.Eval(Container.DataItem,"HR_SALIDA")%></div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="DO">
                                        <HeaderTemplate>
                                            <label style="font-size: xx-small">Do</label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <div style="font-size: xx-small"><%#DataBinder.Eval(Container.DataItem,"SUN")%></div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="LU">
                                        <HeaderTemplate>
                                            <label style="font-size: xx-small">Lu</label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <div style="font-size: xx-small"><%#DataBinder.Eval(Container.DataItem,"MON")%></div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="MA">
                                        <HeaderTemplate>
                                            <label style="font-size: xx-small">Ma</label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <div style="font-size: xx-small"><%#DataBinder.Eval(Container.DataItem,"TUE")%></div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="MI">
                                        <HeaderTemplate>
                                            <label style="font-size: xx-small">Mi</label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <div style="font-size: xx-small"><%#DataBinder.Eval(Container.DataItem,"WED")%></div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="JU">
                                        <HeaderTemplate>
                                            <label style="font-size: xx-small">Ju</label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <div style="font-size: xx-small"><%#DataBinder.Eval(Container.DataItem,"THU")%></div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="VI">
                                        <HeaderTemplate>
                                            <label style="font-size: xx-small">Vi</label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <div style="font-size: xx-small"><%#DataBinder.Eval(Container.DataItem,"FRi")%></div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="SA">
                                        <HeaderTemplate>
                                            <label style="font-size: xx-small">Sa</label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <div style="font-size: xx-small"><%#DataBinder.Eval(Container.DataItem,"SAT")%></div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="SCHD">
                                        <HeaderTemplate>
                                            <label style="font-size: xx-small">Schedule</label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <div style="font-size: xx-small"><%#DataBinder.Eval(Container.DataItem,"SCHD")%></div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="PTRM">
                                        <HeaderTemplate>
                                            <label style="font-size: xx-small">PTRM</label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <div style="font-size: xx-small"><%#DataBinder.Eval(Container.DataItem,"SCHD")%></div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="NIVEL">
                                        <HeaderTemplate>
                                            <label style="font-size: xx-small">NIVEL</label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <div style="font-size: xx-small"><%#DataBinder.Eval(Container.DataItem,"NIVEL")%></div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <RowStyle CssClass="table-row" />
                                <SelectedRowStyle CssClass="gvSelectedRow" Font-Bold="True" ForeColor="#333333" />
                                <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center"></PagerStyle>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>