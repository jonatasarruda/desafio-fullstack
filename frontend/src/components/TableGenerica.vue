<!-- eslint-disable vue/valid-v-slot -->
<template>
  <v-data-table
    :headers="headers"
    :items="items"
    :search="search"
    :loading="loading"
    :items-per-page="itemsPerPage"
    :hide-default-footer="hideDefaultFooter"
    :footer-props="footerProps"
    :item-key="itemKey"
    :class="tableClass"
    v-bind="$attrs" 
    v-on="$listeners"
  >
    <!-- Slot para customizar a renderização de uma célula específica baseada no 'value' do header -->
    <template v-for="header in headers" v-slot:[`item.${header.value}`]="{ item }">
      <slot :name="`item.${header.value}`" :item="item" :value="item[header.value]">
        <!-- Conteúdo padrão da célula -->
        {{ item[header.value] }}
      </slot>
    </template>

    <!-- Slot específico para ações, se você tiver um header com value: 'actions' -->
    <template v-if="hasActionsSlot" v-slot:item.actions="{ item }">
      <slot name="actions" :item="item"></slot>
    </template>

    <!-- Slot para quando não há dados -->
    <template v-slot:no-data>
      <slot name="no-data">
        <v-alert :value="true" color="info" icon="mdi-information-outline" class="ma-3">
          Nenhum dado disponível.
        </v-alert>
      </slot>
    </template>

    <!-- Slot para o topo da tabela (ex: barra de pesquisa externa, botões de adicionar) -->
    <template v-slot:top>
      <slot name="top"></slot>
    </template>

     <!-- Slot para o rodapé da tabela, se o default não for suficiente -->
    <template v-slot:footer>
      <slot name="footer"></slot>
    </template>

  </v-data-table>
</template>

<script>
export default {
  name: 'GenericTable',
  props: {
    headers: {
      type: Array,
      required: true,
      default: () => [] // Ex: [{ text: 'Nome', value: 'nome', sortable: true, align: 'start' }]
    },
    items: {
      type: Array,
      required: true,
      default: () => []
    },
    itemKey: { // Propriedade para a chave única de cada item, importante para performance e seleção
      type: String,
      default: 'id' // Assumindo que seus itens têm um 'id'
    },
    search: {
      type: String,
      default: ''
    },
    loading: {
      type: Boolean,
      default: false
    },
    itemsPerPage: {
      type: Number,
      default: 10
    },
    hideDefaultFooter: {
      type: Boolean,
      default: false
    },
    footerProps: {
      type: Object,
      default: () => ({ 'items-per-page-options': [5, 10, 25, 50, { text: 'Todos', value: -1 }] })
    },
    tableClass: {
      type: String,
      default: ''
    },
    hasActionsSlot: { // Para indicar se o slot de ações nomeado 'actions' será usado
      type: Boolean,
      default: false
    }
  }
}
</script>