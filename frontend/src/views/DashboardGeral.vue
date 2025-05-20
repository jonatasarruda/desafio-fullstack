<template>
  <v-container fluid>
    <!-- Seção de Filtros (Opcional, pode ser adicionada depois) -->
    <h1 class="my-5 px-5">Dashboard Geral</h1>
    <v-row>
      <!-- Novos Gráficos -->
      <v-col cols="12" md="6">
        <v-card>
          <v-card-title>Atendimentos por Cliente</v-card-title>
          <v-card-text>
            <highcharts v-if="!isLoading" :options="chartOptionsAtendimentosPorCliente"></highcharts>
            <v-progress-circular indeterminate color="primary" v-if="isLoading"></v-progress-circular>
          </v-card-text>
        </v-card>
      </v-col>
      <v-col cols="12" md="6">
        <v-card>
          <v-card-title>Atendimentos por Usuário</v-card-title>
          <v-card-text>
            <highcharts v-if="!isLoading" :options="chartOptionsAtendimentosPorUsuario"></highcharts>
            <v-progress-circular indeterminate color="primary" v-if="isLoading"></v-progress-circular>
          </v-card-text>
        </v-card>
      </v-col>
      <v-col cols="12" md="6">
        <v-card>
          <v-card-title>Atendimentos por Período (Mensal)</v-card-title>
          <v-card-text>
            <highcharts v-if="!isLoading" :options="chartOptionsAtendimentosPorPeriodo"></highcharts>
            <v-progress-circular indeterminate color="primary" v-if="isLoading"></v-progress-circular>
          </v-card-text>
        </v-card>
      </v-col>
    </v-row>
  </v-container>
</template>

<script>
import { parseISO, format, startOfMonth, isValid } from 'date-fns';
// import Highcharts from 'highcharts'; // Importe se precisar acessar Highcharts diretamente
// import stockInit from 'highcharts/modules/stock'; // Exemplo de módulo
// if (typeof Highcharts === 'object') {
//   stockInit(Highcharts);
// }

export default {
  name: 'DashboardGeral',
  data() {
    // Mock de dados - em um app real, viria da API
    const mockClientes = [
      { id: 1, nome: 'Cliente Alpha' },
      { id: 2, nome: 'Cliente Beta Soluções' },
      { id: 3, nome: 'Cliente Gamma TI' },
    ];
    const mockUsuarios = [
      { id: 1, nome: 'João Silva', email: 'joao@example.com' },
      { id: 2, nome: 'Maria Oliveira', email: 'maria@example.com' },
      { id: 3, nome: 'Carlos Pereira', email: 'carlos@example.com' },
    ];
    const mockAtendimentos = [
      { id: 1, clienteId: 1, usuarioId: 1, dataCadastro: '2023-01-15T10:00:00Z', status: 'Fechado' },
      { id: 2, clienteId: 2, usuarioId: 2, dataCadastro: '2023-01-20T11:00:00Z', status: 'Aberto' },
      { id: 3, clienteId: 1, usuarioId: 1, dataCadastro: '2023-02-05T14:30:00Z', status: 'Fechado' },
      { id: 4, clienteId: 3, usuarioId: 3, dataCadastro: '2023-02-10T09:00:00Z', status: 'Em Andamento' },
      { id: 5, clienteId: 2, usuarioId: 1, dataCadastro: '2023-03-01T16:00:00Z', status: 'Aberto' },
      { id: 6, clienteId: 1, usuarioId: 2, dataCadastro: '2023-03-12T12:00:00Z', status: 'Fechado' },
      { id: 7, clienteId: 2, usuarioId: 3, dataCadastro: '2023-01-25T08:00:00Z', status: 'Fechado' },
      { id: 8, clienteId: 3, usuarioId: 1, dataCadastro: '2023-03-18T17:00:00Z', status: 'Aberto' },
      { id: 9, clienteId: 1, usuarioId: 2, dataCadastro: '2023-04-02T10:30:00Z', status: 'Em Andamento' },
      { id: 10, clienteId: 2, usuarioId: 1, dataCadastro: '2023-04-05T11:45:00Z', status: 'Fechado' },
    ];

    return {
      isLoading: true,
      clientes: mockClientes,
      usuarios: mockUsuarios,
      atendimentos: mockAtendimentos,

      chartOptionsColumn: {
        chart: { type: 'column' },
        title: { text: 'Vendas Mensais' },
        xAxis: { categories: ['Jan', 'Fev', 'Mar', 'Abr', 'Mai'] },
        yAxis: { title: { text: 'Vendas (R$)' } },
        series: [{
          name: 'Produto A',
          data: [1000, 1200, 800, 1500, 2000]
        }, {
          name: 'Produto B',
          data: [700, 600, 900, 1100, 1300]
        }]
      },
      chartOptionsPie: {
        chart: { type: 'pie' },
        title: { text: 'Distribuição de Categorias' },
        series: [{
          name: 'Percentual',
          data: [
            { name: 'Eletrônicos', y: 45.0 },
            { name: 'Moda', y: 26.8 },
            { name: 'Casa', y: 12.8 },
            { name: 'Outros', y: 15.4 }
          ]
        }]
      },
      chartOptionsAtendimentosPorCliente: {},
      chartOptionsAtendimentosPorUsuario: {},
      chartOptionsAtendimentosPorPeriodo: {},
    };
  },
  created() {
    this.loadDashboardData();
  },
  methods: {
    async loadDashboardData() {
      this.isLoading = true;
      // Em um cenário real, você faria chamadas à API aqui
      // await Promise.all([
      //   apiService.get('/clientes').then(res => this.clientes = res.data),
      //   apiService.get('/usuarios').then(res => this.usuarios = res.data),
      //   apiService.get('/atendimentos').then(res => this.atendimentos = res.data),
      // ]);
      
      // Simula um delay de carregamento
      await new Promise(resolve => setTimeout(resolve, 500));

      this.processAtendimentosPorCliente();
      this.processAtendimentosPorUsuario();
      this.processAtendimentosPorPeriodo();
      this.isLoading = false;
    },

    processAtendimentosPorCliente() {
      const counts = this.atendimentos.reduce((acc, atendimento) => {
        acc[atendimento.clienteId] = (acc[atendimento.clienteId] || 0) + 1;
        return acc;
      }, {});

      const categories = [];
      const data = [];

      this.clientes.forEach(cliente => {
        if (counts[cliente.id]) {
          categories.push(cliente.nome);
          data.push(counts[cliente.id]);
        }
      });

      this.chartOptionsAtendimentosPorCliente = {
        chart: { type: 'bar' },
        title: { text: null }, // Título já está no v-card-title
        xAxis: { categories: categories, title: { text: 'Clientes' } },
        yAxis: { min: 0, title: { text: 'Nº de Atendimentos' }, allowDecimals: false },
        series: [{ name: 'Atendimentos', data: data, colorByPoint: true }],
        legend: { enabled: false },
      };
    },

    processAtendimentosPorUsuario() {
      const counts = this.atendimentos.reduce((acc, atendimento) => {
        acc[atendimento.usuarioId] = (acc[atendimento.usuarioId] || 0) + 1;
        return acc;
      }, {});

      const categories = [];
      const data = [];

      this.usuarios.forEach(usuario => {
        if (counts[usuario.id]) {
          categories.push(usuario.nome || usuario.email); // Usa nome ou email
          data.push(counts[usuario.id]);
        }
      });

      this.chartOptionsAtendimentosPorUsuario = {
        chart: { type: 'column' },
        title: { text: null },
        xAxis: { categories: categories, title: { text: 'Usuários' } },
        yAxis: { min: 0, title: { text: 'Nº de Atendimentos' }, allowDecimals: false },
        series: [{ name: 'Atendimentos', data: data, colorByPoint: true }],
        legend: { enabled: false },
      };
    },

    processAtendimentosPorPeriodo() {
      const countsByMonth = this.atendimentos.reduce((acc, atendimento) => {
        const date = parseISO(atendimento.dataCadastro);
        if (isValid(date)) {
          const monthYear = format(startOfMonth(date), 'MMM/yy'); // Ex: Jan/23
          acc[monthYear] = (acc[monthYear] || 0) + 1;
        }
        return acc;
      }, {});

      const sortedMonths = Object.keys(countsByMonth).sort((a, b) => {
        const [m1, y1] = a.split('/');
        const [m2, y2] = b.split('/');
        const dateA = new Date(`20${y1}`, 'JanFebMarAprMayJunJulAugSepOctNovDec'.split(/(?=[A-Z])/).indexOf(m1));
        const dateB = new Date(`20${y2}`, 'JanFebMarAprMayJunJulAugSepOctNovDec'.split(/(?=[A-Z])/).indexOf(m2));
        return dateA - dateB;
      });
      
      const categories = sortedMonths;
      const data = sortedMonths.map(month => countsByMonth[month]);

      this.chartOptionsAtendimentosPorPeriodo = {
        chart: { type: 'line' },
        title: { text: null },
        xAxis: { categories: categories, title: { text: 'Mês/Ano' } },
        yAxis: { min: 0, title: { text: 'Nº de Atendimentos' }, allowDecimals: false },
        series: [{ name: 'Atendimentos', data: data }],
      };
    }
  }
};
</script>

<style scoped>
/* Estilos específicos para o dashboard, se necessário */
</style>