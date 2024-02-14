<%@ Page Title="" Language="C#" MasterPageFile="~/Layout.master" AutoEventWireup="true" CodeFile="Index.aspx.cs" MaintainScrollPositionOnPostback="true" Inherits="_Default" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager runat="server" ID="scriptManager" />
    <asp:Literal ID="ltlToogleMenu" runat="server"></asp:Literal>


        <div class="main-panel">
                    <!-- Navbar -->
                <nav class="navbar navbar-expand-lg navbar-white navbar-absolute rounded-0">
                    <div class="container-fluid">
                        <div class="navbar-wrapper">
                            <asp:LinkButton ID="btnToogleMenu" OnClick="btnToogleMenu_Click" CssClass="material-icons btnLoad" runat="server">menu_open</asp:LinkButton>
                            <a class="navbar-brand" href="#">
                                <asp:Label ID="lblTituloPantalla" runat="server" Text="Título Pantalla"></asp:Label>
                            </a>
                        </div>

                        <div class="collapse navbar-collapse justify-content-end">
                            <ul class="navbar-nav">
                                <li class="nav-item dropdown">
                                    <a class="nav-link" href="#pablo" id="navbarDropdownProfile" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                                        <i class="material-icons">person</i>
                                        <p class="d-lg-none d-md-block">
                                            Account
                                        </p>
                                    </a>
                                    <div class="dropdown-menu dropdown-menu-right" aria-labelledby="navbarDropdownProfile">
                                        <a class="dropdown-item" href="#">
                                            <asp:Label ID="lblUsername" runat="server" Text="Label"></asp:Label>
                                        </a>
                                        <a class="dropdown-item" href="#">
                                            <b>Administrador: </b> <asp:Label Text="N" ID="lblEsAdministrador" Visible="true" runat="server" />
                                        </a>
                                        <a class="dropdown-item" href="#">
                                            <asp:Label ID="lblRecinto" runat="server" Text="Label"></asp:Label>
                                        </a>
                                        <a class="dropdown-item hidden" href="#">
                                            <asp:Label ID="lblTipoAcceso" runat="server" Text="Label"></asp:Label>
                                        </a>
                                        <a class="dropdown-item" href="#">
                                            <asp:Label ID="lblFechaModificado" runat="server" Text="Label"></asp:Label>
                                        </a>
                                        <a class="dropdown-item" href="#">
                                            <asp:Label ID="lblConnectionName" runat="server" Text="Label"></asp:Label>
                                        </a>
                                    </div>
                                </li>

                            </ul>
                        </div>
                    </div>
                </nav>
        </div>
                <!-- End Navbar -->

    <div class="container">
        <asp:Panel ID="pnlSideBar" Visible="true" runat="server">
                <div class="sidebar" data-color="danger" data-background-color="white" data-image="assets/img/sidebar-3.jpg">
                    <div class="logo">
                        <img src="assets/img/logo.png" class="logo" />
                        <a href="#" class="simple-text logo-normal">DIRECTORIO UAGM <br /><span class="textEmp">(Empleados y <br />Facultad Regular)</span></a>
                        
                    </div>
                    <div class="sidebar-wrapper">
                        <ul class="nav">

                            <li id="BuscarBanner" class="nav-item btnLoad" style="display:none">
                                <a class="nav-link" href="index.aspx">
                                    <i class="fas fa-search"></i>
                                    <p>Buscar</p>
                                    
                                </a>
                            </li>

                             <li id="BuscarAD" class="nav-item btnLoad" runat="server" visible="false">
                                <a class="nav-link" href="buscarEnAD.aspx">
                                    <i class="fas fa-address-book"></i>
                                    <p>Buscar en AD</p>
                                </a>
                            </li>

                             <li id="ddlImportar" runat="server" visible="false" class="nav-item">
                                <a class="nav-link" data-toggle="collapse" href="#plantillas">
                                    <i class="fas fa-cloud-upload-alt"></i>
                                    <p>Importar<b class="caret"></b></p>
                                </a>

                                <div class="collapse" id="buscar">
                                    <ul class="nav">

                                        <li class="nav-item" runat="server" id="importarEstudiantes" visible="false">
                                            <asp:LinkButton ID="btnCrearPlantilla" PostBackUrl="Cargado.aspx" CssClass="nav-link" runat="server">
                                                <span class="sidebar-mini"><i class="fas fa-user-graduate"></i></span>
                                                <span class="sidebar-normal">Estudiantes</span>
                                            </asp:LinkButton>
                                        </li>

                                        <li class="nav-item" runat="server" visible="false" id="importarLaptop">
                                            <asp:LinkButton ID="LinkButton1" PostBackUrl="CargadoSeriaLaptops.aspx" CssClass="nav-link" runat="server">
                                                <span class="sidebar-mini"><i class="fas fa-laptop"></i></span>
                                                <span class="sidebar-normal"># Series</span>
                                            </asp:LinkButton>
                                        </li>

                                    </ul>
                                </div>
                            </li>

                                             <%--Selección Facultas y Admin --%>
    <li>
            <div class="col-sm-4">
                <div class="form-group">
                    <label>Administrativos</label>
                    <asp:RadioButtonList ID="rdlAdminitrativo" CssClass="form-control btnLoad" RepeatDirection="Vertical" runat="server" AutoPostBack="true" OnSelectedIndexChanged="rdlFacultad_SelectedIndexChanged" >
                        <asp:ListItem Value="%">Todos los recintos</asp:ListItem>
                        <asp:ListItem Value="A">Administración Central &nbsp;&nbsp;</asp:ListItem>
                        <asp:ListItem Value="E">Recinto de Carolina&nbsp;&nbsp;</asp:ListItem>
                        <asp:ListItem Value="M">Recinto de Cupey &nbsp;&nbsp;</asp:ListItem>
                        <asp:ListItem Value="T">Recinto de Gurabo  &nbsp;&nbsp;</asp:ListItem>
                        
                    </asp:RadioButtonList>
                </div>
            </div>
            </li>

<li>
              <div class="col-sm-4">
                <div class="form-group">
                    <label>Facultad</label>
                    <asp:RadioButtonList ID="rdlFacultad" CssClass="form-control btnLoad" RepeatDirection="Vertical" runat="server" AutoPostBack="true" OnSelectedIndexChanged="rdlFacultad_SelectedIndexChanged" >
                        <asp:ListItem Value="%">Todos los recintos &nbsp;&nbsp;</asp:ListItem>
                        <asp:ListItem Value="E">Recinto de Carolina&nbsp;&nbsp;</asp:ListItem>
                        <asp:ListItem Value="M">Recinto de Cupey &nbsp;&nbsp;</asp:ListItem>
                        <asp:ListItem Value="T">Recinto de Gurabo  &nbsp;&nbsp;</asp:ListItem>
                    </asp:RadioButtonList>
                </div>
            </div>
    </li>
                            <%-- Close--%>
                            <li class="nav-item">
                                <a class="nav-link" onclick="salirApp();">
                                    <i class="material-icons">exit_to_app</i>
                                    <p>Salir</p>
                                </a>
                            </li>

                        </ul>

                        </div>
            </div>


            </asp:Panel>
    </div>

               

        
    
    
    <div class="card">
        
        <div class="card-body">
            <div style="">
            <div class="row">
                <div class="col-sm-3" style="display:none">
                    <div class="form-group">
                        <label>Institución</label>
                        <asp:DropDownList runat="server" CssClass="form-control" ID="ddlInstitucion">
                            <asp:ListItem Text="Todas" Value="%" />
                            <asp:ListItem Text="Administración Central" Value="A" />
                            <asp:ListItem Text="Recinto de Carolina" Value="E" />
                            <asp:ListItem Text="Recinto de Cupey" Value="M" />
                            <asp:ListItem Text="Recinto de Gurabo" Value="G" />
                            <asp:ListItem Text="Sistema TV" Value="C" />
                            <asp:ListItem Text="Arecibo Observatory" Value="O" />
                        </asp:DropDownList>
                    </div>
                </div>

                <div class="col-sm-3" style="display:none">
                    <div class="form-group">
                        <label>Oficina</label>
                       <asp:TextBox runat="server" CssClass="form-control" ID="txtOficina" placeholder="% Para buscar todos"></asp:TextBox>
                        <cc1:AutoCompleteExtender runat="server" ID="AutoCompleteExtender2"
                            MinimumPrefixLength="4" EnableCaching="true"
                            CompletionSetCount="1" CompletionInterval="1000"
                            ServiceMethod="autoCompleteOficina" DelimiterCharacters=""
                            CompletionListCssClass="completionList"
                            CompletionListItemCssClass="listItem"
                            CompletionListHighlightedItemCssClass="itemHighlighted"
                            Enabled="true"
                            TargetControlID="txtOficina"
                            OnClientPopulated="OnClientCompleted" OnClientPopulating="OnClientPopulating" OnClientHiding="OnClientCompleted">
                        </cc1:AutoCompleteExtender>
                    </div>
                </div>

                <div class="col-sm-3" style="display:none">
                    <div class="form-group">
                        <label>Nombre/Apellido <i class="fas fa-info-circle" data-toggle="tooltip" data-placement="top" title="Escriba y seleccione un nombre de la lista."></i></label>
                        <asp:TextBox runat="server" CssClass="form-control" ID="txtNombre" placeholder="% Para buscar todos"></asp:TextBox>
                        <cc1:AutoCompleteExtender runat="server" ID="AutoCompleteExtender1"
                            MinimumPrefixLength="4" EnableCaching="true"
                            CompletionSetCount="1" CompletionInterval="1000"
                            ServiceMethod="autoCompleteNombre" DelimiterCharacters=""
                            CompletionListCssClass="completionList"
                            CompletionListItemCssClass="listItem"
                            CompletionListHighlightedItemCssClass="itemHighlighted"
                            Enabled="true"
                            TargetControlID="txtNombre"
                            OnClientPopulated="OnClientCompleted" OnClientPopulating="OnClientPopulating" OnClientHiding="OnClientCompleted">
                        </cc1:AutoCompleteExtender>
                    </div>
                </div>

                <div class="col-sm-3" runat="server" visible="false" style="display:none">
                    <div class="form-group">
                        <label>ID usuario</label>
                        <asp:TextBox runat="server" CssClass="form-control" ID="txtId" placeholder=""></asp:TextBox>
                    </div>
                </div>

                <div class="col-sm-3" style="display:none">
                    <div class="form-group">
                        <label>Puesto</label>
                        <asp:TextBox runat="server" CssClass="form-control" ID="txtPuesto" placeholder="% Para buscar todos"></asp:TextBox>
                        <cc1:AutoCompleteExtender runat="server" ID="autoCompleteRecurso"
                            MinimumPrefixLength="4" EnableCaching="true"
                            CompletionSetCount="1" CompletionInterval="1000"
                            ServiceMethod="autoCompletePuesto" DelimiterCharacters=""
                            CompletionListCssClass="completionList"
                            CompletionListItemCssClass="listItem"
                            CompletionListHighlightedItemCssClass="itemHighlighted"
                            Enabled="true"
                            TargetControlID="txtPuesto"
                            OnClientPopulated="OnClientCompleted" OnClientPopulating="OnClientPopulating" OnClientHiding="OnClientCompleted">
                        </cc1:AutoCompleteExtender>
                    </div>
                </div>


                                           

            <asp:Label Text="" ID="lblTotal" runat="server" />
        </div>
            <div class="input-group">
                <asp:TextBox ID="myInputSearch" AutoPostBack="true" CssClass="form-control" Visible="True" OnTextChanged="inputSearch_TextChanged" runat="server" placeholder="Buscar..."  ></asp:TextBox>
                <%--<input type="text" id="myInputSearch" class="form-control" placeholder="Buscar..." />--%>
                <div class="input-group-append">
                    <asp:LinkButton ID="btnBuscar" CssClass="btn btn-success btn-round btnLoad" runat="server" Visible="false" OnClick="btnBuscar_Click"><i class="fas fa-search"></i> Buscar</asp:LinkButton>
                    <a href="Index.aspx" class="btn btn-danger btn-round"><i class="fas fa-sync-alt"></i></a>
                </div>
            </div>

            

            <div class="table-responsive">
                <asp:GridView ID="gvResultados" CssClass="table table-hover" runat="server" AutoGenerateColumns="false" EmptyDataText="No hay datos."
                    BorderStyle="None" BorderWidth="0" AllowSorting ="true"  OnSorting="gvResultados_Sorting" AllowFilteringByColumn="True">
                    <Columns>
                        <asp:BoundField HeaderText="Institución <i class='fa fa-sort-alpha-down'>" SortExpression="INSTITUTION" DataField="INSTITUTION" HtmlEncode="false"  />
                        <asp:BoundField HeaderText="Oficina <i class='fa fa-sort-alpha-down'>" SortExpression="OFFICE"  DataField="OFFICE" HtmlEncode="false" />
                        <%--<asp:BoundField HeaderText="ID" DataField="ID" />--%>
                        <asp:BoundField HeaderText="Nombre <i class='fa fa-sort-alpha-down'>" SortExpression="EMPLOYEE_NAME" DataField="EMPLOYEE_NAME" HtmlEncode="false" />
                        <asp:BoundField HeaderText="Puesto <i class='fa fa-sort-alpha-down'>"  SortExpression="POSITION_TITLE" DataField="POSITION_TITLE" HtmlEncode="false" />
                        <asp:BoundField HeaderText="Email <i class='fa fa-sort-alpha-down'>" SortExpression="EMAIL_AGM" DataFormatString="<a href=mailto:{0}>{0}</a>" DataField="EMAIL_AGM" HtmlEncode="false"></asp:BoundField>
                        <asp:BoundField HeaderText="Extensión <i class='fa fa-sort-amount-down'>" SortExpression="EXTENSION" runat="server" DataField="EXTENSION" HtmlEncode="false" />
                        
                        <asp:HyperLinkField   HeaderText="Conectarse a Teams" runat="server" ControlStyle-CssClass="btn btn-round" Target="_blank" 
                            Text="<i class='fas fa-users style='color:#fff'></i>" DataNavigateUrlFields="TEAMS"></asp:HyperLinkField>
                            
                    </Columns>
                    <RowStyle CssClass="table-row" />
                    <FooterStyle CssClass="footer-grid" />
                    <SelectedRowStyle CssClass="gvSelectedRow" Font-Bold="True" ForeColor="#333333" />
                </asp:GridView>
            </div>
        </div>
    </div>
    <button
        type="button"
        class="btn btn-danger btn-floating btn-lg"
        id="btn-back-to-top"
        style="position:fixed; right:10px; bottom:40px;">
  <i class="fas fa-arrow-up"></i>
</button>
    

    <script>
        function btnConfirmarInsert() {
            swal({
                title: '¿Desea importar los datos?',
                html: 'Tenga en cuenta que este proceso puede tardar varios minutos. No cierre el navegador, no salga de la pantalla ni presione algún control de la aplicación. Espere hasta que se muestre la próxima alerta con el resume de la importación.',
                showCancelButton: true,
                confirmButtonText: 'Sí, estoy de acuerdo',
                cancelButtonText: 'No',
                allowEscapeKey: false,
                allowOutsideClick: false,
                allowEnterKey: false
            }).then((result) => {
                if (result.value) {
                    var obj = document.getElementById('btnInsertar');
                    obj.click();
                    swal({ title: '¡Importando!', text: 'Por favor espere.', type: '' });
                }
            });
        }

        $(document).ready(function () {
            $("#myInputSearch").on("keyup", function () {
                var value = $(this).val().toLowerCase();
                $(".table tr.table-row").filter(function () {
                    $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1)
                });
            });

            $("#BuscarBanner").addClass('active');
        });

        //Get the button
        let mybutton = document.getElementById("btn-back-to-top");

        // When the user scrolls down 20px from the top of the document, show the button
        window.onscroll = function () {
            scrollFunction();
        };

        function scrollFunction() {
            if (
                document.body.scrollTop > 10 ||
                document.documentElement.scrollTop > 10
            ) {
                mybutton.style.display = "block";
            } else {
                mybutton.style.display = "none";
            }
        }
        // When the user clicks on the button, scroll to the top of the document
        mybutton.addEventListener("click", backToTop);

        function backToTop() {
            document.body.scrollTop = 0;
            document.documentElement.scrollTop = 0;
        }
    </script>
    </div>
</asp:Content>

