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
import apiService from '@/services/apiService';
// import Highcharts from 'highcharts'; // Importe se precisar acessar Highcharts diretamente
// import stockInit from 'highcharts/modules/stock'; // Exemplo de módulo
// if (typeof Highcharts === 'object') {
//   stockInit(Highcharts);
// }

export default {
  name: 'DashboardGeral',
  data() {


    return {
      isLoading: true,
      clientes: [], 
      usuarios: [], 
      atendimentos: [], 
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
  
      try {
        const [clientesRes, usuariosRes, atendimentosRes] = await Promise.all([
          apiService.obterTodos('/clientes'), 
          apiService.obterTodos('/usuarios'), 
          apiService.obterTodos('/atendimentos') 
        ]);
        this.clientes = clientesRes;
        this.usuarios = usuariosRes;
        this.atendimentos = atendimentosRes;

        this.processAtendimentosPorCliente();
        this.processAtendimentosPorUsuario();
        this.processAtendimentosPorPeriodo();

      } catch (error) {
        console.error("Erro ao carregar dados do dashboard:", error);
        // Adicionar notificação de erro para o usuário aqui, se desejar
      } finally {
        this.isLoading = false;
      }
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
          categories.push(usuario.nome || usuario.email); 
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