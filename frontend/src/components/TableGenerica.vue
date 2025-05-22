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

    <template v-for="header in headers" v-slot:[`item.${header.value}`]="{ item }">
      <slot :name="`item.${header.value}`" :item="item" :value="item[header.value]">

        {{ item[header.value] }}
      </slot>
    </template>


    <template v-if="hasActionsSlot" v-slot:item.actions="{ item }">
      <slot name="actions" :item="item"></slot>
    </template>


    <template v-slot:no-data>
      <slot name="no-data">
        <v-alert :value="true" color="info" icon="mdi-information-outline" class="ma-3">
          Nenhum dado disponível.
        </v-alert>
      </slot>
    </template>

 
    <template v-slot:top>
      <slot name="top"></slot>
    </template>

     
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
      default: () => [] 
    },
    items: {
      type: Array,
      required: true,
      default: () => []
    },
    itemKey: { 
      type: String,
      default: 'id' 
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
    hasActionsSlot: { 
      type: Boolean,
      default: false
    }
  }
}
</script>