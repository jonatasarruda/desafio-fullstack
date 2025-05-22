<template>
  <v-dialog
    :value="dialog"
    @input="$emit('update:dialog', $event)"
    persistent
    max-width="600px"
    @keydown.esc="closeDialog"
  >
    <v-card v-if="localItem">
      <v-card-title>
        <span class="headline">{{ modalTitle }}</span>
      </v-card-title>
      <v-card-text>
        <v-form ref="form" v-model="validForm" @submit.prevent="saveItem">
          <v-container>
            <v-row>
              <v-col
                v-for="field in formFields"
                :key="field.model"
                :cols="field.cols || 12"
                :sm="field.sm"
                :md="field.md"
              >

                <v-text-field
                  v-if="['text', 'password', 'email', 'number'].includes(field.type)"
                  v-model="localItem[field.model]"
                  :label="field.label"
                  :type="field.type"
                  :required="field.required"
                  :rules="field.rules || (field.required ? [v => !!v || `${field.label} é obrigatório`] : [])"
                  :counter="field.counter"
                  :hint="field.hint"
                  :prepend-icon="field.prependIcon"
                ></v-text-field>


                <v-select
                  v-if="field.type === 'select'"
                  v-model="localItem[field.model]"
                  :items="field.items"
                  :label="field.label"
                  :required="field.required"
                  :rules="field.rules || (field.required ? [v => !!v || `${field.label} é obrigatório`] : [])"
                  :item-text="field.itemText || 'text'"
                  :item-value="field.itemValue || 'value'"
                  :prepend-icon="field.prependIcon"
                ></v-select>

                <slot :name="`field.${field.model}`" :item="localItem" :field="field">
                </slot>
              </v-col>
            </v-row>
          </v-container>
        </v-form>
        <small v-if="formFields.some(f => f.required)">*campos obrigatórios</small>
      </v-card-text>
      <v-card-actions>
        <v-spacer></v-spacer>
        <v-btn color="grey darken-1" text @click="closeDialog">Cancelar</v-btn>
        <v-btn color="blue darken-1" text @click="saveItem" :disabled="!validForm">Salvar</v-btn>
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>

<script>
export default {
  name: 'ModalGenerico',
   model: {
    prop: 'dialog', 
    event: 'update:dialog' 
  },
  props: {
    dialog: { 
      type: Boolean,
      required: true,
    },
    itemToEdit: { 
      type: Object,
      default: null,
    },
    formFields: { 
      type: Array,
      required: true,
      default: () => [],
    },
    modalTitle: {
      type: String,
      default: 'Editar Item',
    },
  },
  data() {
    return {
      localItem: null,
      validForm: true,
    };
  },
  watch: {
    dialog(newVal) {
      if (newVal) {
        this.localItem = this.itemToEdit ? { ...this.itemToEdit } : this.formFields.reduce((obj, field) => {
          obj[field.model] = field.defaultValue !== undefined ? field.defaultValue : null;
          return obj;
        }, {});
        this.$nextTick(() => {
          if (this.$refs.form) this.$refs.form.resetValidation();
        });
      }
    }
  },
  methods: {
    saveItem() {
      if (this.$refs.form.validate()) {
        this.$emit('save', this.localItem);
      }
    },
    closeDialog() {
      this.$emit('update:dialog', false);
    },
  },
};
</script>