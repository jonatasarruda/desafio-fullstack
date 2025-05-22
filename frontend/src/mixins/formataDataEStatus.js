// src/mixins/formataDataEStatus.js
import { format, parseISO, isValid } from 'date-fns';

export default {
  methods: {
    getStatusColor(ativo) {
      return ativo ? 'green' : 'red';
    },
    formatDataCadastro(dataCadastro) {
      if (dataCadastro) {
        const parsedDate = parseISO(dataCadastro);
        if (isValid(parsedDate)) {
          return format(parsedDate, 'dd/MM/yyyy'); 
        }
      }
      return 'Data inválida';
    },
    processSingleItem(item) {
      const ativoBooleano = typeof item.ativo === 'string' 
        ? item.ativo.toLowerCase() === 'true' 
        : !!item.ativo;
      
      return {
        ...item,
        dataCadastro: this.formatDataCadastro(item.dataCadastro),
        ativo: ativoBooleano,
      };
    },
    processItems(items) {
      return items.map(item => this.processSingleItem(item));
    }
  }
};
