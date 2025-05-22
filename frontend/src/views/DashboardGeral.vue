<template>
  <v-container fluid>
    <h1 class="my-5 px-5">Dashboard Geral</h1>
    <v-row>
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
          <v-card-title class="d-flex justify-space-between align-center">
            <span>Atendimentos por Período</span>
            <v-select
              v-model="periodoSelecionado"
              :items="opcoesPeriodo"
              label="Agrupar por"
              dense
              outlined
              hide-details
              class="ml-4"
              style="max-width: 180px; flex-shrink: 0;"
              @change="updateChartThemes"
            ></v-select>
          </v-card-title>
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
import { parseISO, format, startOfMonth, isValid, parse } from 'date-fns';
import apiService from '@/services/apiService';


export default {
  name: 'DashboardGeral',
  data() {

    return {
      isLoading: true,
      clientes: [], 
      usuarios: [], 
      atendimentos: [], 
      periodoSelecionado: 'mensal',
      opcoesPeriodo: [
        { text: 'Mes', value: 'mensal' },
        { text: 'Dia', value: 'diario' },
      ],
      chartOptionsAtendimentosPorCliente: {},
      chartOptionsAtendimentosPorUsuario: {},
      chartOptionsAtendimentosPorPeriodo: {},
      
    };
  },
  created() {
    this.loadDashboardData();
  },
  watch: {
    '$vuetify.theme.dark': {
      handler() {
        this.updateChartThemes(); 
    },
  },
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

        this.processAtendimentosPorCliente(this.$vuetify.theme.dark);
        this.processAtendimentosPorUsuario(this.$vuetify.theme.dark);
        this.processAtendimentosPorPeriodo(this.$vuetify.theme.dark);

      } catch (error) {
        console.error("Erro ao carregar dados do dashboard:", error);

      } finally {
        this.isLoading = false;
      }
    },
 updateChartThemes() {
      try {
        this.processAtendimentosPorCliente(this.$vuetify.theme.dark);
        this.processAtendimentosPorUsuario(this.$vuetify.theme.dark);
        this.processAtendimentosPorPeriodo(this.$vuetify.theme.dark);
      } catch (error) {
        console.error("Erro ao atualizar temas ou período do gráfico:", error);
      }
    },

    processAtendimentosPorCliente(isDark) {
      const counts = this.atendimentos.reduce((acc, atendimento) => {
        acc[atendimento.clienteId] = (acc[atendimento.clienteId] || 0) + 1;
        return acc;
      }, {});

       const textColor = isDark ? '#E0E0E0' : '#333333'; 
      const gridLineColor = isDark ? '#555555' : '#E6E6E6'; 
      const backgroundColor = isDark ? '#1E1E1E' : '#FFFFFF'; 

      const categories = [];
      const data = [];

      this.clientes.forEach(cliente => {
        if (counts[cliente.id]) {
          categories.push(cliente.nome);
          data.push(counts[cliente.id]);
        }
      });

      this.chartOptionsAtendimentosPorCliente = {
        chart: { type: 'bar',
                backgroundColor: backgroundColor
              },
        title: { text: null }, 
       xAxis: { 
            categories: categories, title: { text: 'Clientes', style: { color: textColor } },
            labels: { style: { color: textColor } }, 
        },
        yAxis: { min: 0, title: { text: 'Nº de Atendimentos', style: { color: textColor } }, allowDecimals: false,
            labels: { style: { color: textColor } }, 
            gridLineColor: gridLineColor 
        },    
        series: [{ name: 'Atendimentos', data: data, colorByPoint: true }],
        legend: { enabled: false },
      };
    },

    processAtendimentosPorUsuario(isDark) {
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

      const textColor = isDark ? '#E0E0E0' : '#333333';
      const gridLineColor = isDark ? '#555555' : '#E6E6E6';
      const backgroundColor = isDark ? '#1E1E1E' : '#FFFFFF'

      this.chartOptionsAtendimentosPorUsuario = {
         chart: { 
            type: 'column',
            backgroundColor: backgroundColor 
        },
        title: { text: null },
        xAxis: { 
            categories: categories, title: { text: 'Usuários', style: { color: textColor } },
            labels: { style: { color: textColor } }, 
        },
        yAxis: { min: 0, title: { text: 'Nº de Atendimentos', style: { color: textColor } }, allowDecimals: false,
            labels: { style: { color: textColor } }, 
            gridLineColor: gridLineColor 
        },
        series: [{ name: 'Atendimentos', data: data, colorByPoint: true }],
        legend: { enabled: false },
      };
    },

    processAtendimentosPorPeriodo(isDark) {
      let periodFormatString;
      let xAxisTitle;

      if (this.periodoSelecionado === 'diario') {
        periodFormatString = 'dd/MM/yy'; 
        xAxisTitle = 'Dia';
      } else { // mensal
        periodFormatString = 'MMM/yy'; 
        xAxisTitle = 'Mês/Ano';
      }

      const countsByPeriod = this.atendimentos.reduce((acc, atendimento) => {
        const date = parseISO(atendimento.dataCadastro);
        if (isValid(date)) {
          let periodKey;
          if (this.periodoSelecionado === 'diario') {
            periodKey = format(date, periodFormatString);
          } else { 
            periodKey = format(startOfMonth(date), periodFormatString);
          }
          acc[periodKey] = (acc[periodKey] || 0) + 1;
        }
        return acc;
      }, {});

      let sortedPeriods;
      if (this.periodoSelecionado === 'diario') {
        
        sortedPeriods = Object.keys(countsByPeriod).sort((a, b) => {
          const dateA = parse(a, periodFormatString, new Date());
          const dateB = parse(b, periodFormatString, new Date());
          return dateA.getTime() - dateB.getTime();
        });
      } else { 
        sortedPeriods = Object.keys(countsByPeriod).sort((a, b) => {
         
          const dateA = parse(`01/${a}`, `dd/${periodFormatString}`, new Date());
          const dateB = parse(`01/${b}`, `dd/${periodFormatString}`, new Date());
          return dateA.getTime() - dateB.getTime();
        });
      }
      
      const categories = sortedPeriods;
      const data = sortedPeriods.map(period => countsByPeriod[period]);

      const textColor = isDark ? '#E0E0E0' : '#333333';
      const gridLineColor = isDark ? '#555555' : '#E6E6E6';
      const backgroundColor = isDark ? '#1E1E1E' : '#FFFFFF';

      this.chartOptionsAtendimentosPorPeriodo = {
        chart: {
          type: 'line',
          backgroundColor: backgroundColor,
        },
        title: { text: null },
        xAxis: {
          categories: categories,
          title: { text: xAxisTitle, style: { color: textColor } },
          labels: { style: { color: textColor } },
        },
        yAxis: {
          min: 0,
          title: { text: 'Nº de Atendimentos', style: { color: textColor } },
          allowDecimals: false,
          labels: { style: { color: textColor } },
          gridLineColor: gridLineColor,
        },
        series: [{ name: 'Atendimentos', data: data }],
        legend: {
          itemStyle: { color: textColor },
          itemHoverStyle: { color: isDark ? '#FFFFFF' : '#000000' }
        },
        tooltip: {
          backgroundColor: isDark ? 'rgba(30, 30, 30, 0.85)' : 'rgba(255, 255, 255, 0.85)',
          style: { color: textColor }
        }
      };
    }
  },
};

</script>

<style scoped>

</style>