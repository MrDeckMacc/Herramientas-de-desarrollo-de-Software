<template>
  <v-app id="inspire">
    <!--Boton para el drawer-->
    <v-btn color="blue darken-4" dark @click.stop="drawer = !drawer">
      <v-icon>mdi-menu</v-icon>
    </v-btn>
    <v-navegation-drawer v-model="drawer" app>
      <!--Inicio-->
      <v-list-item :to="{ name: 'home' }">
        <v-list-item-action>
          <v-icon>home</v-icon>
        </v-list-item-action>
        <v-item-tittle>
          Inicio
        </v-item-tittle>
      </v-list-item>
      <!--Almacen-->
      <template>
        <v-list-group>
          <v-item slot="activator">
            <v-item-content>
              <v-list-item-tittle>
                Almacen
              </v-list-item-tittle>
            </v-item-content>
          </v-item>
          <v-list-item :to="{ name: 'categorias' }">
            <v-list-item-action>
              <v-icon>table_chart</v-icon>
            </v-list-item-action>
            <v-item-content>
              <v-item-tittle>
                Gestion de Categorias
              </v-item-tittle>
            </v-item-content>
          </v-list-item>
          <v-list-item :to="{ name: 'articulos' }">
            <v-list-item-action>
              <v-icon>table_chart</v-icon>
            </v-list-item-action>
            <v-item-content>
              <v-item-tittle>
                Gestion de Articulos
              </v-item-tittle>
            </v-item-content>
          </v-list-item>
        </v-list-group>
      </template>
      <!--Compras-->
      <template>
        <v-list-group>
          <v-item slot="activator">
            <v-item-content>
              <v-list-item-tittle>
                Compras
              </v-list-item-tittle>
            </v-item-content>
          </v-item>
          <v-list-item :to="{ name: 'ingresos' }">
            <v-list-item-action>
              <v-icon>table_chart</v-icon>
            </v-list-item-action>
            <v-item-content>
              <v-item-tittle>
                Acciones de Ingresos
              </v-item-tittle>
            </v-item-content>
          </v-list-item>
          <v-list-item :to="{ name: 'proveedores' }">
            <v-list-item-action>
              <v-icon>table_chart</v-icon>
            </v-list-item-action>
            <v-item-content>
              <v-item-tittle>
                Gestion de proveedores
              </v-item-tittle>
            </v-item-content>
          </v-list-item>
        </v-list-group>
      </template>
      <!--Ventas-->
      <template>
        <v-list-group>
          <v-item slot="activator">
            <v-item-content>
              <v-list-item-tittle>
                Ventas
              </v-list-item-tittle>
            </v-item-content>
          </v-item>
          <v-list-item :to="{ name: 'ventas' }">
            <v-list-item-action>
              <v-icon>table_chart</v-icon>
            </v-list-item-action>
            <v-item-content>
              <v-item-tittle>
                Acciones de Ventas
              </v-item-tittle>
            </v-item-content>
          </v-list-item>
          <v-list-item :to="{ name: 'clientes' }">
            <v-list-item-action>
              <v-icon>table_chart</v-icon>
            </v-list-item-action>
            <v-item-content>
              <v-item-tittle>
                Gestion de Clientes
              </v-item-tittle>
            </v-item-content>
          </v-list-item>
        </v-list-group>
      </template>
      <!--Usuarios-->
      <template>
        <v-list-group>
          <v-item slot="activator">
            <v-item-content>
              <v-list-item-tittle>
                Usuarios
              </v-list-item-tittle>
            </v-item-content>
          </v-item>
          <v-list-item :to="{ name: 'roles' }">
            <v-list-item-action>
              <v-icon>table_chart</v-icon>
            </v-list-item-action>
            <v-item-content>
              <v-item-tittle>
                Gestion de Roles
              </v-item-tittle>
            </v-item-content>
          </v-list-item>
          <v-list-item :to="{ name: 'usuarios' }">
            <v-list-item-action>
              <v-icon>table_chart</v-icon>
            </v-list-item-action>
            <v-item-content>
              <v-item-tittle>
                Gestion de Usuarios
              </v-item-tittle>
            </v-item-content>
          </v-list-item>
        </v-list-group>
      </template>
      <!--Consultas-->
      <template>
        <v-list-group>
          <v-item slot="activator">
            <v-item-content>
              <v-list-item-tittle>
                Consultas
              </v-list-item-tittle>
            </v-item-content>
          </v-item>
          <v-list-item :to="{ name: 'compras' }">
            <v-list-item-action>
              <v-icon>table_chart</v-icon>
            </v-list-item-action>
            <v-item-content>
              <v-item-tittle>
                Consultar Compras
              </v-item-tittle>
            </v-item-content>
          </v-list-item>
          <v-list-item :to="{ name: 'ventas' }">
            <v-list-item-action>
              <v-icon>table_chart</v-icon>
            </v-list-item-action>
            <v-item-content>
              <v-item-tittle>
                Consultar Ventas
              </v-item-tittle>
            </v-item-content>
          </v-list-item>
        </v-list-group>
      </template>
    </v-navegation-drawer>
    <!--Encabezado-->
    <v-header>
      <v-card height="100" width="100%" class="text-center white--text" color="blue darken-4">
        <h3 class="pt-10"> Sistema para el control de Ventas</h3>
      </v-card>
    </v-header>
    <!--Contenido-->
    <v-main>
      <h1 class="text-center success--text mt-10">
        Bienvenidos al sistema de control de ventas Versión
      </h1>
      <router-view></router-view>
    </v-main>
    <!--Pie de Pagina-->
    <v-footer app>
      <v-card height="30" width="100%" class="text-center white--text" color="blue darken-4">
        <h3> ISyTE - FIAD-UABC - {{ new Date().getFullYear() }}</h3>
      </v-card>
    </v-footer>
  </v-app>
</template>

<script>
export default {
  name: 'App',

  data: () => ({
    drawer: false,
  }),
};
</script>
