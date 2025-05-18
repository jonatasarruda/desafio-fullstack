<!-- eslint-disable vue/valid-v-slot -->
<template>
  <v-container fluid>
    <v-card class="my-5 mx-5">
      <v-card-title>
        Controle de Usuários
        <v-spacer></v-spacer>
      </v-card-title>
      <TableGenerica
        :headers="userHeaders"
        :items="userItems"
        :loading="isLoading"
        :search="searchTerm"
        item-key="id"
        table-class="elevation-1"
        :has-actions-slot="true" 
      >
        
        <template v-slot:top>
        <div class="d-flex justify-content-center">
          <v-text-field
            v-model="searchTerm"
            label="Buscar Usuário..."
            class = "mx-2 px-2"
            prepend-inner-icon="mdi-magnify"
            clearable
          ></v-text-field>
           <v-btn
            elevation="2"
            raised
            @click="cadastroUsuario()"
            color = primary
            class = "mx-2 px-2"
            >Cadastrar Novo Usuário</v-btn> 
        </div>
        </template>
        
        <template v-slot:item.status="{ item }">
          <v-chip :color="getStatusColor(item.status)" small dark>
            {{ item.status === 'active' ? 'Ativo' : 'Inativo' }}
          </v-chip>
        </template>

        <template v-slot:actions="{ item }">
          <v-tooltip bottom>
            <template v-slot:activator="{ on, attrs }">
              <v-btn icon small v-bind="attrs" v-on="on" @click="editarUsuario(item)">
                <v-icon small color="primary">mdi-pencil</v-icon>
              </v-btn>
            </template>
            <span>Editar Usuário</span>
          </v-tooltip>
          <v-tooltip bottom>
            <template v-slot:activator="{ on, attrs }">
              <v-btn icon small v-bind="attrs" v-on="on" @click="excluirUsuario(item)">
                <v-icon small color="error">mdi-delete</v-icon>
              </v-btn>
            </template>
            <span>Excluir Usuário</span>
          </v-tooltip>
        </template>

        <template v-slot:no-data>
          <v-alert color="warning" icon="mdi-alert" class="ma-4">
            Nenhum usuário encontrado com os critérios de busca.
          </v-alert>
        </template>

      </TableGenerica>
    </v-card>
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

export default {
  name: 'UsuarioView',
  components: {
    TableGenerica,
    ModalGenerico
  },
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
        // { text: 'Status', value: 'status', sortable: false, align: 'center' },
        { text: 'Data de cadastro', value: 'dataCadastro', sortable: true, align: 'center' },
        { text: 'Ações', value: 'actions', sortable: false, align: 'right', width: '150px' } 
      ],
      userItems: [],
      userFormFields: [
    
        { model: 'email', label: 'Email', type: 'text', required: true, rules: [
            v => !!v || 'Email é obrigatório',
            v => /.+@.+\..+/.test(v) || 'Email deve ser válido',
          ], cols: 12 },
        { model: 'senha', label: 'Senha', type: 'text', required: true, cols: 12 },

        // { model: 'status', label: 'Status*', type: 'select', required: true,
        //   items: [ { text: 'Ativo', value: 'active' },{ text: 'Inativo', value: 'inactive' }],
        //   itemText: 'text', // Chave para o texto de exibição no select
        //   itemValue: 'value', // Chave para o valor no select
        //   cols: 12,
        // },
      ],
    };
  },
  methods: {
    getStatusColor(status) {
      return status === 'active' ? 'green' : 'red';
    },
    editarUsuario(usuario) {
      this.exibirModal.currentItem = usuario; 
      usuario.senha = '';
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
      // Lógica para confirmar e excluir o usuário
    },
    async handleSaveUser(savedItem) {
        if (this.exibirModal.novoCadastro) {
            apiService.cadastrarNovo('/usuarios', savedItem)
            .then(response => {
                this.userItems.push({...response})
                console.log('Usuário cadastrado:', response);
        })
        } else {
        const index = this.userItems.findIndex(u => u.id === savedItem.id);
        if (index !== -1) {
            
            await apiService.atualizar('/usuarios', savedItem.id, savedItem)
                      .then(response =>{
                        Object.assign(this.userItems[index], response)
                      })
            console.log('Usuário atualizado:', this.userItems[index]);
        }
    }
      this.exibirModal.show = false;
    },
  },
  async created() {
    let resultadoUsuarios = await apiService.obterTodos('/usuarios');

    resultadoUsuarios.forEach( usuario =>
        {
            this.userItems.push({...usuario})
        }
    )

    }
  }

</script>

<style scoped>
v-text-field {
    padding-top: 0px;
}
</style>
