<template>
<div fluid>
  <v-card>
    <v-app-bar
      absolute
      color="primary"
      elevate-on-scroll
      scroll-target="#scrolling-techniques-7"
      app
    >
      <v-spacer></v-spacer>

      <v-btn icon dark class="ml-auto">
        <v-icon>mdi-heart</v-icon>
      </v-btn>

      <v-btn icon dark>
        <v-icon>mdi-dots-vertical</v-icon>
      </v-btn>
    </v-app-bar>
    <v-sheet
      id="scrolling-techniques-7"
      class="overflow-y-auto"
      max-height="600"
    >
    </v-sheet>
  </v-card>
  <div class="fill-height">
      <v-navigation-drawer
      v-model="drawer"
      :mini-variant.sync="mini"
      permanent
      fixed
      color="primary"
      app
    >
      <v-list-item class="px-2 my-4" dark>

        
        <v-list-item-title class="my-4" v-show="!mini">John Leider</v-list-item-title>

        <v-btn
          icon
          @click.stop="mini = !mini"
        >
          <v-icon>mdi-chevron-left</v-icon>
        </v-btn>
      </v-list-item>


      <v-list >
        <v-list-item
          v-for="item in itemsMenu"
          :key="item.title"
          link
          :to="item.path"
          dark
        >
          <v-list-item-icon>
            <v-icon>{{ item.icon }}</v-icon>
          </v-list-item-icon>

          <v-list-item-content>
            <v-list-item-title>{{ item.title }}</v-list-item-title>
          </v-list-item-content>
        </v-list-item>
      </v-list>
    </v-navigation-drawer>

  </div>
</div>
</template>

<script>

  export default {
    name: 'MenuSistema',
    data () {
      return {
        drawer: true,
        itemsMenu: [],
        mini: true,
      }
    },
    created() {
      this.carregarItensMenu();
    },
    methods: {
      carregarItensMenu() {
        const rotasDoMenu = this.$router.options.routes
          .filter(rota => rota.meta && rota.meta.showInMenu) // Filtra rotas que devem aparecer no menu
          .map(rota => {
            return {
              title: rota.meta.title || rota.name, // Usa o título do meta ou o nome da rota
              icon: rota.meta.icon || 'mdi-help-circle-outline', // Ícone do meta ou um padrão
              path: rota.path, // Caminho da rota para a navegação
            };
          });
        this.itemsMenu = rotasDoMenu;
      }
    }
  }
</script>