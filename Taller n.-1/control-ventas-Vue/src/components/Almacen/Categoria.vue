<h1>Roles</h1>
<template>
  <v-data-table :headers="headers" :items="categorias" :search="search" sort-by="nombreCategorias" class="elevation-1">
    <template v-slot:top>
      <v-toolbar flat>
        <v-toolbar-title>Categorias</v-toolbar-title>
        <v-divider class="mx-4" inset vertical></v-divider>
        <v-spacer></v-spacer>
        <!-- Busqueda de categoria -->
        <v-text-field class="texte-center" v-model="search" append-icon="search" label="Busqueda" single-line hide-details></v-text-field>
        <v-divider class="mx-4" inset vertical></v-divider>
        <v-spacer></v-spacer>
        <v-dialog v-model="dialog" max-width="500px">
          <template v-slot:activator="{ on, attrs }">
            <v-btn color="primary" dark class="mb-2" v-bind="attrs" v-on="on">
              Nueva Categoria
            </v-btn>
          </template>
          <v-card>
            <v-card-title>
              <span class="text-h5">{{formTitle}}</span>
            </v-card-title>

            <v-card-text>
              <v-container>
                <v-row>
                  <v-col cols="12" sm="6" md="4">
                    <v-text-field v-model="nombreCategoria" label="Nombre Categoria"></v-text-field>
                  </v-col>
                  <v-col cols="12" sm="6" md="4">
                    <v-text-field v-model="descripcionCategoria" label="Descripción Categoria"></v-text-field>
                  </v-col>
                </v-row>
              </v-container>
            </v-card-text>

            <v-card-actions>
              <v-spacer></v-spacer>
              <v-btn color="blue darken-1" text @click="close">
                Cancelar
              </v-btn>
              <v-btn color="blue darken-1" text @click="Grabar">
                Grabar
              </v-btn>
            </v-card-actions>
          </v-card>
        </v-dialog>
        <!-- Modal para validar si se quiere activar o desactivar la categoria-->
        <v-dialog v-model="adModal" max-width="350px">
        <v-card>
          <v-card-title class="headline" v-if="adAccion==1">¿Activar Categoria?</v-card-title>
          <v-card-title class="headline" v-if="adAccion==2">¿Desactivar Categoria?</v-card-title>
          <v-card-text>Vas a 
            <span v-if="adAccion==1">Activar</span>
            <span v-if="adAccion==2">Desactivar</span>
            la categoría {{adNombre}}
          </v-card-text>
          <v-card-actions>
            <v-spacer></v-spacer>
            <v-btn color="blue darken-1" text @click="activarDesactivarCerrar">Cancelar</v-btn>
            <v-btn v-if="adAccion==1" color="success darken-1" text @click="activar">Activar</v-btn>
            <v-btn v-if="adAccion==2" color="red darken-1" text @click="desactivar">Desactivar</v-btn>
          </v-card-actions>
        </v-card>
        </v-dialog>
        <!-- Modal -->
        <v-dialog v-model="dialogDelete" max-width="500px">
          <v-card>
            <v-card-title class="text-h5">Estas seguro de que quieres eliminar esta categoria?</v-card-title>
            <v-card-actions>
              <v-spacer></v-spacer>
              <v-btn color="blue darken-1" text @click="closeDelete">Cancelar</v-btn>
              <v-btn color="blue darken-1" text @click="deleteItemConfirm">Grabar</v-btn>
              <v-spacer></v-spacer>
            </v-card-actions>
          </v-card>
        </v-dialog>
      </v-toolbar>
    </template>
    <!--Botones para las acciones de editar, Activar y Desactivar -->
    <template v-slot:item.actions="{ item }">
      <v-icon medium color="primary darken-1" class="mr-2" size="x-large" @click="editItem(item)">mdi-pincel</v-icon>
        
      <template v-if="item.estado">
        <v-icon medium color="green darken-2" class="mr-2" size="x-large" @click="modalActivarDesactivar(2, item)">check</v-icon>
      </template>
      <template v-else>
        <v-icon medium color="red darken-2" class="mr-2" size="x-large" @click="modalActivarDesactivar(1, item)">cancel</v-icon>
      </template>
    </template>
    <template v-slot:no-data>
      <v-btn color="primary" @click="initialize">
        Reset
      </v-btn>
    </template>
  </v-data-table>
</template>


<script>
import axios from 'axios'

export default {
  data: () => ({
    search: '',
    categorias: [],
    nombreCategoria:[],
    descripcionCategoria:[],
    adModal: 0,
    adAccion: 0,
    adNombre: '',
    adIdCategoria: '',

    validar() {
      this.valida=0;
      this.ValidaMensajes=[];

      if(this.nombreCategoria.length< 3 || this.nombreCategoria.length >100) //aqui solamente agregué el .lengh en la segunda condicion
        this.ValidaMensajes.push("El nombre del la Categoria debe tener más de 3 caracteres y menos de 100");
      if(this.ValidaMensajes.length)
      this.valida=1;

      return this.valida;
    },

    dialog: false,
    dialogDelete: false,
    headers: [
      { text: 'Nombre Categoria', value: 'nombreCategoria', align:'start', sortable:true },
      { text: 'Descripcion Categoria', value: 'descripcion',sortable:true },
      { text: 'Estado', value: 'estado',sortable:true },
      { text: 'Accion', value: 'actions', sortable: false },
    ],
    
    editedIndex: -1,
    editedItem: {
      idCategoria: '',
      nombreCategoria: '',
      descripcion: '',
    },
  }),

  computed: {
    formTitle() {
      return this.editedIndex === -1 ? 'Nuevo Categoria' : 'Editar Categoria'
    },
  },

  watch: {
    dialog(val) {
      val || this.close()
    },
    dialogDelete(val) {
      val || this.closeDelete()
    },
  },

  created() {
    this,this.ListadoCategorias();
    this.initialize()
  },
//44338
  methods: {
    ListadoCategorias()
    {
        let Lista = this;
        axios.get('https://localhost:44338/api/Categorias/ListarCategorias').then(function(response)
        {
            console.log(response);
            Lista.categorias = response.data;
        }).catch(function(error)
          {
            console.log(error);
          })
        ;
    },



    initialize() {
     
    },

    editItem(item) {
      this.idCategoria = item.idCategoria;
      this.nombreCategoria = item.nombreCategoria;
      this.descripcion = item.descripcion;
      this.editedIndex = 1;
      this.dialog = true
    },

    deleteItem(item) {
      this.editedIndex = this.desserts.indexOf(item)
      this.editedItem = Object.assign({}, item)
      this.dialogDelete = true
    },

    deleteItemConfirm() {
      this.desserts.splice(this.editedIndex, 1)
      this.closeDelete()
    },

    close() {
      this.dialog = false
      this.$nextTick(() => {
        this.editedItem = Object.assign({}, this.defaultItem)
        this.editedIndex = -1
      })
    },

    closeDelete() {
      this.dialogDelete = false
      this.$nextTick(() => {
        this.editedItem = Object.assign({}, this.defaultItem)
        this.editedIndex = -1
      })
    },

    Grabar() {
      if(this.validar() )
      {
        return;
      }
      if (this.editedIndex > -1) {
        let me = this;
        axios.put('/api/Categorias/ModificarCategoria/5', 
        {
          'idCategoria': me.idCategoria,
          'nombreCategoria': me.nombreCategoria,
          'descripcion': me.descripcion
        }).then(function(response){
          me.close();
          me.ListadoCategorias();
          me.LimpiarModal();
        }).catch(function(error)
        {
          console.log(error);
        });

        //Sección para editar los datos 
      } else {
        //Sección para Guardar los datos de una nuevo rol
        let me = this;
        axios.post('api/Categorias/InsertarCategoria', 
        {
          'nombreCategoria': me.nombreCategoria,
          'descripcion': me.descripcionCategoria,
        }).then(function(response){
          me.close();
          me.ListadoCategorias();
          me.LimpiarModal();
        }).catch(function(error)
        {
          console.log(error);
        });
      }
      this.close()
    },

    LimpiarModal() {
      this.idCategoria= '';
      this.nombreCategoria='';
      this.descripcion='';
    },

    modalActivarDesactivar(accion, item) {
      this.adModal = 1;
      this.adIdCategoria = item.idCategoria;
      this.adNombre = item.nombreCategoria;
      
      if(accion==1){
        this.adAccion = 1;
      }
      else if (accion==2){
        this.adAccion = 2;
      } 
      else{
        this.adModal = 0;
      }
    },

    activar(){
      let me = this;
      axios.put('api/Categorias/ActivarCategoria/'+this.adIdCategoria,{}).then(function(response){
        me.adModal=0;
        me.adAccion=0;
        me.adNombre='';
        me.adIdCategoria=0;
        me.close();
        me.ListadoCategorias();
      }).catch(function(error) {
        console.log(error);
      });
    },

    desactivar(){
      let me = this;
      axios.put('api/Categorias/DesactivarCategoria/'+this.adIdCategoria,{}).then(function(response){
        me.adModal=0;
        me.adAccion=0;
        me.adIdCategoria=0;
        me.close();
        me.ListadoCategorias();
      }).catch(function(error) {
        console.log(error);
      });
    },

    activarDesactivarCerrar(){
      this.adModal=0;
    }

  },
}
</script>