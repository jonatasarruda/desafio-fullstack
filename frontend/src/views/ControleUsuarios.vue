<!-- eslint-disable vue/valid-v-slot -->
<template>
  <v-container fluid>
    <h1 class="my-5 px-5">Controle de usuários</h1>
    
    <v-card class="my-5 mx-5">
      <TableGenerica
        :headers="userHeaders"
        :items="userItems"
        :loading="isLoading"
        :search="searchTerm"
        item-key="id"
        table-class="elevation-1 custom-row-height-table"
        :has-actions-slot="true" 
      >
        
      <template v-slot:top>
        <div class="d-flex justify-content-center py-5">
          <v-text-field
            v-model="searchTerm"
            label="Buscar Usuário..."
            class = "mx-8 "
            prepend-inner-icon="mdi-magnify"
            clearable
          ></v-text-field>
          <div class= "py-2">
            <v-btn
              v-if="!$vuetify.breakpoint.smAndDown"
              elevation="2"
              raised
              @click="cadastroUsuario()"
              color="primary"
              class="mx-5 py-5"
            >Cadastrar Novo Usuário</v-btn>

          </div> 
        </div>
      </template>
        
        <template v-slot:item.ativo="{ item }">
          <v-chip :color="getStatusColor(item.ativo)" dark>
            {{ item.ativo ? 'Ativo' : 'Inativo' }}
          </v-chip>
        </template>

        <template v-slot:actions="{ item }">
          <v-tooltip bottom>
            <template v-slot:activator="{ on, attrs }">
              <v-btn icon v-bind="attrs" v-on="on" @click.stop="editarUsuario(item)">
              <v-icon color="secondary">mdi-pencil</v-icon>
              </v-btn>
            </template>
            <span>Editar Usuário</span>
          </v-tooltip>
        </template>

        <template v-slot:no-data>
          <v-alert color="warning" icon="mdi-alert" class="ma-4">
            Nenhum usuário encontrado com os critérios de busca.
          </v-alert>
        </template>

      </TableGenerica>
    </v-card>

    <v-tooltip left>
      <template v-slot:activator="{ on, attrs }">
        <v-btn
          v-if="$vuetify.breakpoint.smAndDown"
          fab
          dark
          small
          fixed
          bottom
          right
          color="primary"
          class="ma-4" 
          elevation="4"
          @click="cadastroUsuario()" 
          v-bind="attrs"
          v-on="on"
        >
          <v-icon>mdi-plus</v-icon>
        </v-btn>
      </template>
      <span>Cadastrar Novo Usuário</span>
    </v-tooltip>

    <ModalGenerico
      v-model="exibirModal.show"
      :item-to-edit="exibirModal.currentItem"
      :form-fields="userFormFields"
      :modal-title="exibirModal.title"
      @save="handleSaveUser"
    />
  </v-container>
</template>

<script>
import ModalGenerico from '@/components/ModalGenerico.vue';
import TableGenerica from '@/components/TableGenerica.vue';
import apiService from '@/services/apiService.js';
import formataDataEStatus from '@/mixins/formataDataEStatus'; 

export default {
  name: 'UsuarioView',
  components: {
    TableGenerica,
    ModalGenerico
  },
  mixins: [formataDataEStatus], 
  data() {
    return {
      isLoading: false,
      searchTerm: '',
      exibirModal: { 
        show: false,
        title: '',
        currentItem: null,
        novoCadastro: false,
      },
      userHeaders: [
        { text: 'ID', value: 'id', sortable: true, width: '10%' },
        { text: 'Email', value: 'email', sortable: true},
        { text: 'Status', value: 'ativo', sortable: false, align: 'center' },
        { text: 'Data de cadastro', value: 'dataCadastro', sortable: true, align: 'center' },
        { text: 'Ações', value: 'actions', sortable: false, align: 'right', width: '150px' } 
      ],
      userItems: [],
      userFormFields: [
    
        { model: 'email', label: 'Email', type: 'text', required: true, rules: [
            v => !!v || 'Email é obrigatório',
            v => /.+@.+\..+/.test(v) || 'Email deve ser válido',
          ], cols: 12 },
        { model: 'senha', label: 'Senha', type: 'password', required: true, cols: 12 },

        { model: 'ativo', label: 'Status*', type: 'select', required: true,
          items: [ { text: 'Ativo', value: true },{ text: 'Inativo', value: false }],
          itemText: 'text', 
          itemValue: 'value', 
          cols: 12,
           rules: [
            v => typeof v === 'boolean' || 'Status é obrigatório. Selecione Ativo ou Inativo.'
          ]
        },
      ],
    };
  },
  methods: {

    editarUsuario(usuario) {

      this.exibirModal.currentItem = { ...usuario, ativo: !!usuario.ativo, senha: '' }; 

      this.exibirModal.title = `Editar Usuário:`;
      this.exibirModal.novoCadastro = false;
      this.exibirModal.show = true;
    },
    cadastroUsuario() {
      this.exibirModal.currentItem = null;
      this.exibirModal.title = 'Adicionar Novo Usuário';
      this.exibirModal.novoCadastro = true;
      this.exibirModal.show = true
    },
    excluirUsuario(user) {
      console.log('Excluir usuário:', user);
    },
    async handleSaveUser(savedItem) {
        if (this.exibirModal.novoCadastro) {
          const payload = { ...savedItem, ativo: !!savedItem.ativo };
            apiService.cadastrarNovo('/usuarios', payload)
            .then(response => { 
                const processedNewUser = this.processSingleItem(response);
                this.userItems.push(processedNewUser); 
                this.$store.dispatch('snackbar/showSnackbar', {
                  message: `Usuário ${response.email} cadastrado com sucesso!`,
                  color: 'success',
                });
        })
        } else {
        const index = this.userItems.findIndex(u => u.id === savedItem.id);
        if (index !== -1) {
            const originalItem = this.userItems[index]; 
            
            const payload = { ...savedItem, ativo: !!savedItem.ativo };
            await apiService.atualizar('/usuarios', savedItem.id, payload)
                      .then(responseFromServer =>{
                       
                       Object.assign(this.userItems[index], {
                         ...responseFromServer, 
                         ativo: !!responseFromServer.ativo, 
                         dataCadastro: originalItem.dataCadastro 
                       });
                       this.$store.dispatch('snackbar/showSnackbar', {
                         message: `Usuário ${responseFromServer.email} atualizado com sucesso!`,
                         color: 'success',
                       });
                      })
        } else{
           console.error("Usuário não encontrado na lista para atualização:", savedItem.id);
            this.$store.dispatch('snackbar/showSnackbar', {
              message: 'Erro: Usuário não encontrado para atualização.',
              color: 'error',
            });
        }
    }
      this.exibirModal.show = false;
    },
  },
  async created() {
    this.isLoading = true;
    try {
      const resultadoUsuariosApi = await apiService.obterTodos('/usuarios');
      this.userItems = this.processItems(resultadoUsuariosApi);
    } catch (error) {
      console.error("Erro ao carregar usuários:", error);
      this.$store.dispatch('snackbar/showSnackbar', {
        message: 'Erro ao carregar usuários.',
        color: 'error',
        timeout: 5000,
      });
    } finally {
      this.isLoading = false;
    }
  }
}
</script>

<style scoped>
v-text-field {
    padding-top: 0px;
}
.custom-row-height-table .v-data-table__wrapper > table > tbody > tr > td {
  height: 59px !important;
 }
</style>
