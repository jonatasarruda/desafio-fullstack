<!-- eslint-disable vue/valid-v-slot -->
<template>
  <v-container fluid class="pa-0 ma-0 fill-height-container">
    <v-row class="fill-height ma-0">
      <v-col cols="12" class="pa-0 fill-height">
        <v-window v-model="activeWindow" class="fill-height" touchless>
          <v-window-item :value="0" class="fill-height pa-0">
            <v-card class="fill-height d-flex flex-column" style="overflow-y: auto;">
              <v-card-title>
                Lista de Atendimentos
                <v-spacer></v-spacer>
              </v-card-title>              
              <TableGenerica
                :headers="atendimentoHeaders"
                :items="atendimentoItensProcessados"
                :loading="isLoadingTable"
                :search="searchTerm"
                item-key="id"
                table-class="elevation-1"
                :has-actions-slot="true"
                @click:row="cliqueLinha"
                class="flex-grow-1"
              >
                <template v-slot:top>
                  <v-expansion-panels flat class="mb-0 pb-0">
                    <v-expansion-panel>
                      <v-expansion-panel-header class="pa-3">
                        <div class="d-flex align-center" style="width: 100%;">
                          <v-text-field
                            v-model="searchTerm"
                            label="Buscar por ID ou Descrição..."
                            class="mx-2 flex-grow-1"
                            prepend-inner-icon="mdi-magnify"
                            clearable
                            dense
                            hide-details
                            @click.native.stop
                          ></v-text-field>
                          <!-- Botão para telas maiores (md e acima) -->
                          <v-btn
                            v-if="!$vuetify.breakpoint.smAndDown"
                            @click.native.stop="inicarNovoAtendimento()"
                            color="primary"
                            class="mx-2"
                            elevation="2"
                          >
                            Novo Atendimento
                          </v-btn>

                        </div>
                      </v-expansion-panel-header>
                      <v-expansion-panel-content class="pt-0">
                        <v-row class="pa-2 align-center" dense>
                          <v-col cols="12" md="3">
                            <v-text-field 
                            v-model="filtros.dataInicio" 
                            label="Data Início" 
                            type="date" 
                            dense 
                            outlined 
                            hide-details 
                            clearable></v-text-field>
                          </v-col>
                          <v-col cols="12" md="3">
                            <v-text-field 
                            v-model="filtros.dataFim" 
                            label="Data Fim" 
                            type="date" 
                            dense 
                            outlined 
                            hide-details 
                            clearable 
                            :min="filtros.dataInicio"></v-text-field>
                          </v-col>
                          <v-col cols="12" md="3">
                            <v-select 
                            v-model="filtros.clienteId" 
                            :items="clientesList" 
                            item-text="nome" 
                            item-value="id" 
                            label="Cliente" 
                            dense 
                            outlined 
                            hide-details 
                            clearable></v-select>
                          </v-col>
                          <v-col cols="12" md="3">
                            <v-select 
                            v-model="filtros.usuarioId" 
                            :items="usuariosList" 
                            item-text="email" 
                            item-value="id" 
                            label="Usuário" 
                            dense 
                            outlined 
                            hide-details 
                            clearable></v-select>
                          </v-col>
                        </v-row>
                        <v-row class="pa-2 pb-3" dense>
                            <v-col class="d-flex justify-end">
                                <v-btn text small color="secondary" @click="limparFiltros" class="mr-2">Limpar Filtros</v-btn>
                            </v-col>
                        </v-row>
                      </v-expansion-panel-content>
                    </v-expansion-panel>
                  </v-expansion-panels>
                </template>

                <template v-slot:item.status="{ item }">
                  <v-chip :color="getStatusColor(item.status)" small dark>
                    {{ formatStatus(item.status) }}
                  </v-chip>
                </template>

                <template v-slot:actions="{ item }">
                  <v-tooltip bottom>
                    <template v-slot:activator="{ on, attrs }">
                      <v-btn icon small v-bind="attrs" v-on="on" @click.stop="navegarParaDetalhes(item, 'view')">
                        <v-icon small color="info">mdi-eye</v-icon>
                      </v-btn>
                    </template>
                    <span>Visualizar Detalhes</span>
                  </v-tooltip>
                  <v-tooltip bottom>
                    <template v-slot:activator="{ on, attrs }">
                      <v-btn icon small v-bind="attrs" v-on="on" @click.stop="navegarParaDetalhes(item, 'edit')" :disabled="item.status === 'cancelado' || item.status === 'concluido'">
                        <v-icon small color="primary">mdi-pencil</v-icon>
                      </v-btn>
                    </template>
                    <span>Editar Atendimento</span>
                  </v-tooltip>
                </template>

                <template v-slot:no-data>
                  <v-alert color="warning" icon="mdi-alert" class="ma-4">
                    Nenhum atendimento encontrado.
                  </v-alert>
                </template>
              </TableGenerica>

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
                    @click="inicarNovoAtendimento()" 
                    v-bind="attrs"
                    v-on="on"
                  >
                    <v-icon>mdi-plus</v-icon>
                  </v-btn>
                </template>
                <span>Novo Atendimento</span>
              </v-tooltip>
            </v-card>
          </v-window-item>

          <v-window-item :value="1" class="fill-height pa-0">
            <v-card v-if="atendimentoAtual" class="fill-height d-flex flex-column">
              <v-card-title>
                <v-btn icon @click="navigateToListView" class="mr-2">
                  <v-icon>mdi-arrow-left</v-icon>
                </v-btn>
                {{ tituloFormView }}
                <v-spacer></v-spacer>
                <v-btn v-if="visualizacaoDetalhes === 'view'" color="primary" @click="edicaoAtendimento" class="mr-2" :disabled="atendimentoAtual && atendimentoAtual.status === 'cancelado' || atendimentoAtual && atendimentoAtual.status === 'concluido'">
                  <v-icon left>mdi-pencil</v-icon>Editar
                </v-btn>
                <v-btn v-if="visualizacaoDetalhes === 'edit' || visualizacaoDetalhes === 'create'" outlined color="error" @click="cancelarEdicaoAtendimento" class="mr-2">
                  Cancelar
                </v-btn>
                <v-btn v-if="visualizacaoDetalhes === 'edit' || visualizacaoDetalhes === 'create'" color="primary" @click="salvarAtendimento" :loading="isSaving">
                  <v-icon left>mdi-content-save</v-icon>Salvar
                </v-btn>
              </v-card-title>
              <v-divider></v-divider>
              <v-card-text class="flex-grow-1 overflow-y-auto">
                <v-row class="fill-height">                  

                  <!-- Coluna Esquerda: Informações do Atendimento -->
                  <v-col md="7" cols="12">
                    <h3 class="mb-3">Dados do Atendimento</h3>
                    <v-form ref="detailFormRef" :readonly="visualizacaoDetalhes === 'view'">
                      <v-select
                        v-model="atendimentoAtual.clienteId"
                        :items="clientesListAtivos"
                        item-text="nome"
                        item-value="id"
                        label="Cliente"
                        :rules="[v => !!v || 'Cliente é obrigatório']"
                        required
                        outlined
                        dense
                        :readonly="visualizacaoDetalhes === 'view'"
                      ></v-select>

                      <v-select
                        v-model="atendimentoAtual.usuarioId"
                        :items="usuariosListAtivos"
                        item-text="email"
                        item-value="id"
                        label="Usuário Responsável"
                        :rules="[v => !!v || 'Usuário é obrigatório']"
                        required
                        outlined
                        dense
                        :readonly="visualizacaoDetalhes === 'view'"
                      ></v-select>

                      <v-textarea
                        v-model="atendimentoAtual.descricao"
                        label="Descrição do Atendimento"
                        :rules="[v => !!v || 'Descrição é obrigatória']"
                        required
                        outlined
                        dense
                        rows="3"
                        :readonly="visualizacaoDetalhes === 'view'"
                      ></v-textarea>

                      <v-text-field
                        v-model="atendimentoAtual.dataAtendimento"
                        label="Data do Atendimento"
                        type="date"
                        :rules="[v => !!v || 'Data é obrigatória']"
                        required
                        outlined
                        dense
                        :readonly="visualizacaoDetalhes === 'view'"
                      ></v-text-field>

                      <v-select
                        v-model="atendimentoAtual.status"
                        :items="statusAtendimentoList"
                        item-text="text"
                        item-value="value"
                        label="Status"
                        :rules="[v => !!v || 'Status é obrigatório']"
                        required
                        outlined
                        dense
                        :readonly="visualizacaoDetalhes === 'view'"
                      ></v-select>
                    </v-form>
                  </v-col>

                  <!-- Coluna Direita: Pareceres -->

                  <v-col md="5" cols="12">
                    <h3 class="mb-3">Pareceres do Atendimento</h3>
                    <v-progress-linear indeterminate color="primary" v-if="isLoadingPareceres"></v-progress-linear>
                    <div v-if="!isLoadingPareceres && pareceres.length === 0 && visualizacaoDetalhes !== 'create'">
                      <v-alert type="info" dense outlined>Nenhum parecer registrado.</v-alert>
                    </div>
                    <v-list dense v-if="!isLoadingPareceres && pareceres.length > 0" class="pa-0">
                      <template v-for="(parecer, index) in pareceres">
                        <v-list-item :key="parecer.id">
                          <v-list-item-content v-if="editingParecerId !== parecer.id">
                            <v-list-item-title v-text="parecer.texto"></v-list-item-title>
                            <v-list-item-subtitle>{{ parecer.autor }} - {{ parecer.data | formatDate }}</v-list-item-subtitle>
                          </v-list-item-content>
                          <v-list-item-action v-if="editingParecerId !== parecer.id && atendimentoAtual && atendimentoAtual.id && atendimentoAtual.status !== 'cancelado'">
                            <v-tooltip bottom>
                              <template v-slot:activator="{ on, attrs }">
                                <v-btn icon small v-bind="attrs" v-on="on" @click="iniciaEdicaoParecer(parecer)">
                                  <v-icon small>mdi-pencil</v-icon>
                                </v-btn>
                              </template>
                              <span>Editar Parecer</span>
                            </v-tooltip>
                          </v-list-item-action>

                          <!-- Modo de Edição do Parecer -->
                          <v-list-item-content v-if="editingParecerId === parecer.id">
                            <v-textarea
                              v-model="editedParecerText"
                              label="Editar parecer"
                              outlined
                              dense
                              rows="3"
                              hide-details="auto"
                              class="mb-2"
                              :rules="[v => !!(v && v.trim()) || 'O texto do parecer não pode ser vazio']"
                            ></v-textarea>
                            <div class="d-flex align-center">
                              <v-btn small color="primary" @click="saveEditedParecer" :loading="isSavingParecer" :disabled="!editedParecerText || !editedParecerText.trim()">Salvar</v-btn>
                              <v-btn small text @click="cancelaEdicaoParecer" class="ml-2">Cancelar</v-btn>
                            </div>
                            <v-list-item-subtitle class="mt-2">
                              Editando parecer de {{ parecer.autor }} - {{ parecer.data | formatDate }}
                            </v-list-item-subtitle>
                          </v-list-item-content>
                        </v-list-item>
                        <v-divider v-if="index < pareceres.length - 1" :key="`divider-${parecer.id}`"></v-divider>
                      </template>
                    </v-list>
                    <div v-if="atendimentoAtual && atendimentoAtual.id && visualizacaoDetalhes !== 'create' && atendimentoAtual.status !== 'cancelado' 
                    || atendimentoAtual && atendimentoAtual.id && visualizacaoDetalhes !== 'create' && atendimentoAtual.status !== 'concluido'" class="mt-4">
                      <h4 class="mb-2">Adicionar Novo Parecer</h4>
                      <v-textarea
                        v-model="novoParecerTexto"
                        label="Digite seu parecer aqui..."
                        outlined
                        dense
                        rows="3"
                        hide-details="auto"
                        class="mb-2"
                      ></v-textarea>
                      <v-btn
                        color="secondary"
                        small
                        @click="adicionarNovoParecer"
                        :disabled="!novoParecerTexto.trim()"
                        :loading="isAddingParecer"
                      >Adicionar Parecer</v-btn>
                    </div>
                  </v-col>
                </v-row>
              </v-card-text>
            </v-card>
            <v-skeleton-loader v-else-if="isLoadingOverall && activeWindow === 1" type="article, actions" class="ma-2 fill-height"></v-skeleton-loader>
            <v-alert v-else-if="activeWindow === 1" type="info" class="ma-5">Selecione um atendimento na lista para ver os detalhes ou clique em "Novo Atendimento".</v-alert>
          </v-window-item>
        </v-window>
      </v-col>
    </v-row>
    <v-overlay :value="isLoadingOverall">
      <v-progress-circular indeterminate size="64"></v-progress-circular>
    </v-overlay>
  </v-container>
</template>

<script>
import TableGenerica from '@/components/TableGenerica.vue';
import apiService from '@/services/apiService.js';
import { format, parseISO, isValid } from 'date-fns';

export default {
  name: 'ControleAtendimentos',
  components: {
    TableGenerica,
  },
  filters: {
    formatDate(value) {
      if (!value) return '';
      const date = parseISO(value);
      return isValid(date) ? format(date, 'dd/MM/yyyy HH:mm') : 'Data inválida';
    }
  },
  data() {
    return {
      isLoadingTable: false,
      isLoadingOverall: false,
      isSaving: false,
      searchTerm: '',
      activeWindow: 0, // 0 for list, 1 for detail
      visualizacaoDetalhes: 'view', // 'view', 'edit', 'create'
      atendimentoAtual: null,
      originalatendimentoAtual: null,
      atendimentoHeaders: [
        { text: 'ID', value: 'id', sortable: true, width: '8%' },
        { text: 'Cliente', value: 'clienteNome', sortable: true },
        { text: 'Usuário', value: 'usuarioEmail', sortable: true },
        { text: 'Data', value: 'dataAtendimentoFormatted', sortable: true },
        { text: 'Status', value: 'status', sortable: true, align: 'center' },
        { text: 'Ações', value: 'actions', sortable: false, align: 'center', width: '150px' } // Aumentei um pouco a largura para os 3 botões
      ],
      atendimentoItems: [],
      clientesList: [],
      usuariosList: [],
      statusAtendimentoList: [
        { text: 'Aberto', value: 'aberto' },
        { text: 'Em Andamento', value: 'em_andamento' },
        { text: 'Concluído', value: 'concluido' },
        { text: 'Cancelado', value: 'cancelado' }
      ],
      pareceres: [],
      isLoadingPareceres: false,
      novoParecerTexto: '',
      isAddingParecer: false,
      editingParecerId: null,
      editedParecerText: '',
      isSavingParecer: false,
      filtros: {
        dataInicio: null,
        dataFim: null,
        clienteId: null,
        usuarioId: null,
      },
    };
  },
  computed: {
    tituloFormView() {
      if (!this.atendimentoAtual) return 'Atendimento';
      if (this.visualizacaoDetalhes === 'create') return 'Novo Atendimento';
      if (this.visualizacaoDetalhes === 'edit') return `Editar Atendimento: #${this.atendimentoAtual.id}`;
      if (this.visualizacaoDetalhes === 'view') return `Detalhes do Atendimento: #${this.atendimentoAtual.id}`;
      return 'Atendimento';
    },
    clientesListAtivos() {
      return this.clientesList.filter(cliente => cliente.ativo);
    },
    usuariosListAtivos() {
      return this.usuariosList.filter(usuario => usuario.ativo);
    },


    itensFiltrados() {
      let items = [...this.atendimentoItems]; 

      if (this.filtros.dataInicio) {
        items = items.filter(at => at.dataAtendimento && at.dataAtendimento >= this.filtros.dataInicio);
      }
      if (this.filtros.dataFim) {
        items = items.filter(at => at.dataAtendimento && at.dataAtendimento <= this.filtros.dataFim);
      }
      if (this.filtros.clienteId) {
        items = items.filter(at => at.clienteId === this.filtros.clienteId);
      }
      if (this.filtros.usuarioId) {
        items = items.filter(at => at.usuarioId === this.filtros.usuarioId);
      }
      return items;
    },
    atendimentoItensProcessados() {
      return this.itensFiltrados.map(at => { 
        const cliente = this.clientesList.find(c => c.id === at.clienteId);
        const usuario = this.usuariosList.find(u => u.id === at.usuarioId);
        let dataFormatada = 'N/D';
        if (at.dataAtendimento) {
            const parsedDate = parseISO(at.dataAtendimento);
            if(isValid(parsedDate)) {
                dataFormatada = format(parsedDate, 'dd/MM/yyyy');
            }
        }
        return {
          ...at,
          clienteNome: cliente ? cliente.nome : 'N/D',
          usuarioEmail: usuario ? usuario.email : 'N/D',
          dataAtendimentoFormatted: dataFormatada,
        };
      });
    }
  },
  methods: {
    formatStatus(status) {
        const foundStatus = this.statusAtendimentoList.find(s => s.value === status);
        return foundStatus ? foundStatus.text : status;
    },
    getStatusColor(status) {
      const colors = { em_andamento: 'orange', concluido: 'green', cancelado: 'red' };
      return colors[status] || 'grey';
    },
    async cargaIncial() {
      this.isLoadingTable = true;
      this.isLoadingOverall = true;
      try {
        const [atendimentosApi, clientesApi, usuariosApi] = await Promise.all([
          apiService.obterTodos('/atendimentos'), 
          apiService.obterTodos('/clientes'),   
          apiService.obterTodos('/usuarios')    
        ]);
    
        this.atendimentoItems = atendimentosApi.map(apiAt => {
          let dataFormatadaParaInput = null;
          if (apiAt.dataCadastro && typeof apiAt.dataCadastro === 'string') { 
            const parsed = parseISO(apiAt.dataCadastro); 
            if (isValid(parsed)) {
              dataFormatadaParaInput = format(parsed, 'yyyy-MM-dd');
            }
          }
          return { 
            ...apiAt, 
            dataAtendimento: dataFormatadaParaInput 
          };
        });
        this.clientesList = clientesApi;
        this.usuariosList = usuariosApi;
      } catch (error) {
        console.error("Erro ao carregar dados iniciais:", error);
        // TODO: Adicionar notificação de erro para o usuário (ex: snackbar)
      } finally {
        this.isLoadingTable = false;
        this.isLoadingOverall = false;
      }
    },

    navigateToListView() {
      this.activeWindow = 0;
      this.atendimentoAtual = null;
      this.originalatendimentoAtual = null;
      this.visualizacaoDetalhes = 'view';
      this.pareceres = [];
      this.editingParecerId = null;
      this.editedParecerText = '';
      this.isSavingParecer = false;
      this.novoParecerTexto = '';
      this.$refs.detailFormRef?.resetValidation();
      // Não limpamos os filtros da lista principal aqui
    },

    async navegarParaDetalhes(atendimento, mode = 'view') {
      this.isLoadingOverall = true;
      let atendimentoData = { 
        ...atendimento, 
        descricao: atendimento.textoAberturaAtendimento || '' 
      };

      if (atendimentoData.dataAtendimento && typeof atendimentoData.dataAtendimento === 'string' && !/^\d{4}-\d{2}-\d{2}$/.test(atendimentoData.dataAtendimento)) { // Verifica se não é YYYY-MM-DD
        const parsedDate = parseISO(atendimentoData.dataAtendimento); 

        if (isValid(parsedDate)) { 
            atendimentoData.dataAtendimento = format(parsedDate, 'yyyy-MM-dd');
        } else {
            atendimentoData.dataAtendimento = null; 
        }
      } else if (!atendimentoData.dataAtendimento && mode === 'create') { 

        atendimentoData.dataAtendimento = format(new Date(), 'yyyy-MM-dd'); 

      } else if (mode === 'create') {

         atendimentoData.dataAtendimento = format(new Date(), 'yyyy-MM-dd');
         atendimentoData.status = 'aberto';
      }


      this.atendimentoAtual = { ...atendimentoData };
      this.originalatendimentoAtual = JSON.parse(JSON.stringify(atendimentoData)); // Deep copy
      this.visualizacaoDetalhes = mode;
      
      this.pareceres = []; // Limpa pareceres antigos
      if (mode !== 'create' && atendimento.id) {
        await this.carregaPareceres(atendimento.id);
      }
      
      this.activeWindow = 1;
      this.$nextTick(() => {
        this.$refs.detailFormRef?.resetValidation();
      });
      this.isLoadingOverall = false;
    },

    cliqueLinha(item, { event }) {
        if (event && event.target) {
            const targetTagName = event.target.tagName.toLowerCase();
            const parentButton = event.target.closest('button');
            if (targetTagName === 'button' || targetTagName === 'i' || parentButton) {
                return;
            }
        }
        this.navegarParaDetalhes(item, 'view');
    },

    inicarNovoAtendimento() {
      const newAtendimentoShell = {
        clienteId: null,
        usuarioId: null,
        descricao: '',
        dataAtendimento: format(new Date(), 'yyyy-MM-dd'),
        status: 'aberto',
      };
      this.navegarParaDetalhes(newAtendimentoShell, 'create');
    },

    edicaoAtendimento() {
      if (this.atendimentoAtual && this.atendimentoAtual.status === 'cancelado' || this.atendimentoAtual && this.atendimentoAtual.status === 'concluido') {
        // TODO: Adicionar uma notificação para o usuário informando que não pode editar um atendimento cancelado.
        return;
      }
      if (this.atendimentoAtual) { // Se não estiver cancelado, permite a edição
        this.visualizacaoDetalhes = 'edit';
      }
    },

    cancelarEdicaoAtendimento() {
      if (this.visualizacaoDetalhes === 'edit' && this.originalatendimentoAtual) {
        this.atendimentoAtual = JSON.parse(JSON.stringify(this.originalatendimentoAtual));
        this.visualizacaoDetalhes = 'view';
      } else if (this.visualizacaoDetalhes === 'create') {
        this.navigateToListView();
      }
      this.$refs.detailFormRef?.resetValidation();
    },

    async salvarAtendimento() {
      if (!this.$refs.detailFormRef?.validate()) return;

      this.isSaving = true;
      try {
        let atendimentoSalvo;
        const payload = { 
          ...this.atendimentoAtual, 
          textoAberturaAtendimento: this.atendimentoAtual.descricao,
          // Ao enviar para a API, o campo de data do formulário (this.atendimentoAtual.dataAtendimento)
          // deve ser mapeado para o campo que a API espera para a data principal, que é 'dataCadastro'.
          dataCadastro: this.atendimentoAtual.dataAtendimento 
        };
        delete payload.descricao; // Remove a propriedade 'descricao' do frontend antes de enviar
        delete payload.dataAtendimento; // Remove dataAtendimento do payload, pois enviaremos como dataCadastro

        if (this.visualizacaoDetalhes === 'create') {
          atendimentoSalvo = await apiService.cadastrarNovo('/atendimentos', payload);
          // A API retorna atendimentoSalvo.dataCadastro (provavelmente ISO)
          // Precisamos formatá-lo para YYYY-MM-DD para a lista e para o formulário
          let dataFormatadaInputSalvo = null;
          if(atendimentoSalvo.dataCadastro && typeof atendimentoSalvo.dataCadastro === 'string') { // <--- USA dataCadastro DA RESPOSTA DA API
            const parsed = parseISO(atendimentoSalvo.dataCadastro); // <--- USA dataCadastro DA RESPOSTA DA API
            if(isValid(parsed)) dataFormatadaInputSalvo = format(parsed, 'yyyy-MM-dd');
          }

          const newItem = { 
            ...atendimentoSalvo, 
            // Usa a data formatada para o input no item da lista
            dataAtendimento: dataFormatadaInputSalvo
          };
          this.atendimentoItems.push(newItem);
          
          this.atendimentoAtual = { ...newItem, descricao: newItem.textoAberturaAtendimento || '' };
          this.originalatendimentoAtual = JSON.parse(JSON.stringify(this.atendimentoAtual));
          this.visualizacaoDetalhes = 'view';
        } else if (this.visualizacaoDetalhes === 'edit' && this.atendimentoAtual.id) {
          atendimentoSalvo = await apiService.atualizar('/atendimentos', this.atendimentoAtual.id, payload);
          // A API retorna atendimentoSalvo.dataCadastro (provavelmente ISO)
          // Precisamos formatá-lo para YYYY-MM-DD para a lista e para o formulário
          let dataFormatadaInputSalvo = null;
          if(atendimentoSalvo.dataCadastro && typeof atendimentoSalvo.dataCadastro === 'string') { // <--- USA dataCadastro DA RESPOSTA DA API
            const parsed = parseISO(atendimentoSalvo.dataCadastro); // <--- USA dataCadastro DA RESPOSTA DA API
            if(isValid(parsed)) dataFormatadaInputSalvo = format(parsed, 'yyyy-MM-dd');
          }

          const index = this.atendimentoItems.findIndex(at => at.id === this.atendimentoAtual.id);
          if (index !== -1) {
            const updatedItem = { 
              ...atendimentoSalvo, 
              // Usa a data formatada para o input no item da lista
              dataAtendimento: dataFormatadaInputSalvo
            };
            this.$set(this.atendimentoItems, index, updatedItem);
            this.atendimentoAtual = { ...updatedItem, descricao: updatedItem.textoAberturaAtendimento || '' };
          } else {
            // Fallback se o item não for encontrado na lista (improvável, mas seguro)
            this.atendimentoAtual = { ...atendimentoSalvo, descricao: atendimentoSalvo.textoAberturaAtendimento || '' };
          }
          this.originalatendimentoAtual = JSON.parse(JSON.stringify(this.atendimentoAtual));
          this.visualizacaoDetalhes = 'view';
        }
      } catch (error) {
        console.error('Erro ao salvar atendimento:', error);
        // TODO: Adicionar notificação de erro
      } finally {
        this.isSaving = false;
      }
    },

    async carregaPareceres(atendimentoId) {
      if (!atendimentoId) return;
      this.isLoadingPareceres = true;
      try {

        const atendimentoComPareceres = await apiService.obterPorId('/atendimentos', atendimentoId); // Assumindo que obterPorId busca um único atendimento
        
        if (atendimentoComPareceres && atendimentoComPareceres.pareceres) {
          // Assumindo que os pareceres também podem ser modelados se você tiver um Parecer.js
          // Por agora, vamos apenas mapear os campos como antes.
          this.pareceres = atendimentoComPareceres.pareceres.map(apiParecer => {
            const usuario = this.usuariosList.find(u => u.id === apiParecer.usuarioId);
            return {
              ...apiParecer, // Se tiver um modelo Parecer, seria new Parecer(apiParecer)
              texto: apiParecer.descricaoParecer, 
              autor: usuario ? usuario.email : apiParecer.usuarioId, 
              data: apiParecer.dataCadastro 
            };
          });
        } else {
          this.pareceres = []; 
        }
      } catch (error) {
        console.error(`Erro ao carregar pareceres para o atendimento ${atendimentoId}:`, error);
        this.pareceres = [];
        // TODO: Notificação de erro
      } finally {
        this.isLoadingPareceres = false;
      }
      },

    async adicionarNovoParecer() {
      if (!this.novoParecerTexto.trim() || !this.atendimentoAtual || !this.atendimentoAtual.id) {
        // TODO: Adicionar notificação para o usuário (ex: texto do parecer é obrigatório)
        return;
      }

      this.isAddingParecer = true;
      try {
         const userInfoString = localStorage.getItem('user-info');
         const userIdString = localStorage.getItem('user-id');
        if (!userInfoString) {
          console.error("Informações do usuário não encontradas. Faça login novamente.");
          // TODO: Notificar o usuário e talvez redirecionar para o login
          this.isAddingParecer = false;
          return;
        }

        const userId = JSON.parse(userIdString);

        const novoParecerPayload = {
          atendimentoId: this.atendimentoAtual.id,
          descricaoParecer: this.novoParecerTexto, 
          usuarioId: userId,
        };


       const parecerSalvoDaApi = await apiService.cadastrarNovo('/pareceres', novoParecerPayload);

        const parecerSalvoParaFrontend = { 
          ...this.mapParecerDaAPIparaFront(parecerSalvoDaApi)
        };
        this.pareceres.push(parecerSalvoParaFrontend);
        this.novoParecerTexto = '';
        // TODO: Notificação de sucesso
      } catch (error) {
        console.error("Erro ao adicionar novo parecer:", error);
        // TODO: Adicionar notificação de erro para o usuário
      } finally {
        this.isAddingParecer = false;
      }
    },
    mapParecerDaAPIparaFront(apiParecer) {
      const usuario = this.usuariosList.find(u => u.id === apiParecer.usuarioId);
      return {
        id: apiParecer.id,
        atendimentoId: apiParecer.atendimentoId,
        texto: apiParecer.descricaoParecer,
        autor: usuario ? (usuario.nome || usuario.email) : apiParecer.usuarioId,
        data: apiParecer.dataCadastro, 
        usuarioId: apiParecer.usuarioId, 
      };
    },
    iniciaEdicaoParecer(parecer) {
      this.editingParecerId = parecer.id;
      this.editedParecerText = parecer.texto;
    },

    cancelaEdicaoParecer() {
      this.editingParecerId = null;
      this.editedParecerText = '';
    },

    async saveEditedParecer() {
      if (!this.editedParecerText || !this.editedParecerText.trim() || !this.editingParecerId) {
        // TODO: Notificação para o usuário (texto do parecer é obrigatório)
        return;
      }
      this.isSavingParecer = true;
      try {
        const userIdString = localStorage.getItem('user-id');
        const userId = JSON.parse(userIdString);
        const payload = {
         atendimentoId: this.atendimentoAtual.id,
         descricaoParecer: this.editedParecerText, 
         usuarioId: userId,
        };

        
        const parecerAtualizadoDaApi = await apiService.atualizar('/pareceres', this.editingParecerId, payload);

        const index = this.pareceres.findIndex(p => p.id === this.editingParecerId);
        if (index !== -1) {
          const parecerAtualizadoParaFrontend = this.mapParecerDaAPIparaFront(parecerAtualizadoDaApi);
          this.$set(this.pareceres, index, parecerAtualizadoParaFrontend);
        }
        this.cancelaEdicaoParecer();
        // TODO: Notificação de sucesso
      } catch (error) {
        console.error("Erro ao salvar parecer editado:", error);
        // TODO: Adicionar notificação de erro
      } finally {
        this.isSavingParecer = false;
      }
    },

    limparFiltros() {
      this.filtros = {
        dataInicio: null,
        dataFim: null,
        clienteId: null,
        usuarioId: null,
      };
    }
  },
   watch: {
   
    '$root.tokenKey'() { 
        this.cargaIncial();
    }
   },
  created() {
    this.cargaIncial();
  }
}
</script>

<style scoped>

.fill-height-container {
  min-height: 100vh;
}

.fill-height {
  height: 100%;
}


.v-card__text.overflow-y-auto {
  flex-grow: 1; 
  overflow-y: auto; 
}


.v-window-item.fill-height > .v-card.fill-height.d-flex.flex-column {
  overflow-y: auto;
}
</style>