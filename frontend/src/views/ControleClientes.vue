<!-- eslint-disable vue/valid-v-slot -->
<template>
  <v-container fluid>
    <h1 class="my-5 px-5">Cadastro de clientes</h1>
    <v-card class="my-5 mx-5">
      <TableGenerica
        :headers="clientHeaders"
        :items="clientItems"
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
            label="Buscar Clientes..."
            class = "mx-8"
            prepend-inner-icon="mdi-magnify"
            clearable
          ></v-text-field>
          <div class= "py-2">
            <!-- Botão para telas maiores (md e acima) -->
            <v-btn
              v-if="!$vuetify.breakpoint.smAndDown"
              elevation="2"
              raised
              @click="cadastroCliente()"
              color = primary
              class = "mx-5 py-5"
            >Cadastrar Novo Cliente</v-btn>
          </div> 
        </div>
        </template>
        
        <template v-slot:item.ativo="{ item }">
          <v-chip :color="getStatusColor(item.ativo)" small dark>
            {{ item.ativo ? 'Ativo' : 'Inativo' }}
          </v-chip>
        </template>

        <template v-slot:actions="{ item }">
          <v-tooltip bottom>
            <template v-slot:activator="{ on, attrs }">
              <v-btn icon small v-bind="attrs" v-on="on" @click="editarCliente(item)">
                <v-icon small color="primary">mdi-pencil</v-icon>
              </v-btn>
            </template>
            <span>Editar Cliente</span>
          </v-tooltip>
          <v-tooltip bottom>
            <template v-slot:activator="{ on, attrs }">
              <v-btn icon small v-bind="attrs" v-on="on" @click="excluirCliente(item)">
                <v-icon small color="error">mdi-delete</v-icon>
              </v-btn>
            </template>
            <span>Excluir Cliente</span>
          </v-tooltip>
        </template>

        <template v-slot:no-data>
          <v-alert color="warning" icon="mdi-alert" class="ma-4">
            Nenhum cliente encontrado com os critérios de busca.
          </v-alert>
        </template>

      </TableGenerica>
    </v-card>

    <!-- Botão FAB para telas menores (sm e abaixo) -->
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
          @click="cadastroCliente()" 
          v-bind="attrs"
          v-on="on"
        >
          <v-icon>mdi-plus</v-icon>
        </v-btn>
      </template>
      <span>Cadastrar Novo Cliente</span>
    </v-tooltip>

    <ModalGenerico
      v-model="exibirModal.show"
      :item-to-edit="exibirModal.currentItem"
      :form-fields="clientFormFields"
      :modal-title="exibirModal.title"
      @save="handleSaveClient"
    />
  </v-container>
</template>

<script>
import ModalGenerico from '@/components/ModalGenerico.vue';
import TableGenerica from '@/components/TableGenerica.vue';
import apiService from '@/services/apiService.js';
import formataDataEStatus from '@/mixins/formataDataEStatus'; // Importa o mixin

export default {
  name: 'ClienteView',
  components: {
    TableGenerica,
    ModalGenerico
  },
  mixins: [formataDataEStatus], // Usa o mixin
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
      clientHeaders: [
        { text: 'ID', value: 'id', sortable: true, width: '10%' },
        { text: 'Nome', value: 'nome', sortable: true},
        { text: 'Status', value: 'ativo', sortable: false, align: 'center' }, // Alterado value para 'ativo'
        { text: 'Data de cadastro', value: 'dataCadastro', sortable: true, align: 'center' },
        { text: 'Ações', value: 'actions', sortable: false, align: 'right', width: '150px' } 
      ],
      clientItems: [],
      clientFormFields: [
    
        { model: 'nome', label: 'Nome', type: 'text', required: true, rules: [
            v => !!v || 'Nome é obrigatório',
          ], cols: 12 },
        
        { model: 'ativo', label: 'Status*', type: 'select', required: true, // Alterado model para 'ativo'
          items: [ { text: 'Ativo', value: true },{ text: 'Inativo', value: false }], // Alterado para booleanos
          itemText: 'text', // Chave para o texto de exibição no select
          itemValue: 'value', // Chave para o valor no select
          cols: 12,
        },
      ],
    };
  },
  methods: {
    // getStatusColor é agora fornecido pelo mixin
    editarCliente(cliente) {
      this.exibirModal.currentItem = { ...cliente, ativo: !!cliente.ativo }; 
      this.exibirModal.title = `Editar Cliente:`;
      this.exibirModal.novoCadastro = false;
      this.exibirModal.show = true;
    },
    cadastroCliente() {
      this.exibirModal.currentItem = null;
      this.exibirModal.title = 'Adicionar Novo Cliente';
      this.exibirModal.novoCadastro = true;
      this.exibirModal.show = true
    },
    excluirCliente(user) {
      console.log('Excluir usuário:', user);
      // Lógica para confirmar e excluir o usuário
    },
    async handleSaveClient(savedItem) {
        if (this.exibirModal.novoCadastro) {
            const payload = { ...savedItem, ativo: !!savedItem.ativo };
            apiService.cadastrarNovo('/clientes', payload)
            .then(response => {
                this.clientItems.push({...response, ativo: !!response.ativo});
                console.log('Cliente cadastrado:', response);
        })
        } else {
        const index = this.clientItems.findIndex(u => u.id === savedItem.id);
        if (index !== -1) {
            const originalItem = this.clientItems[index];
            const payload = { ...savedItem, ativo: !!savedItem.ativo };
            await apiService.atualizar('/clientes', savedItem.id, payload)
                      .then(responseFromServer =>{
                        Object.assign(this.clientItems[index], {
                          ...responseFromServer,
                          ativo: !!responseFromServer.ativo,
                          dataCadastro: originalItem.dataCadastro 
                        });
                      })
            console.log('Cliente atualizado:', this.clientItems[index]);
        } else {
          console.error("Cliente não encontrado na lista para atualização:", savedItem.id);
          // Adicionar notificação de erro para o usuário
        }
    }
      this.exibirModal.show = false;
    },
  },
  async created() {
    let resultadoClientes = await apiService.obterTodos('/clientes');
    this.clientItems = this.processItems(resultadoClientes); // Usa o método do mixin


    }
  }

</script>

<style scoped>
v-text-field {
    padding-top: 0px;
}
</style>
