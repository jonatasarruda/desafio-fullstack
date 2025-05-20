<template>
<div fluid>
    <v-app-bar
      absolute
      color="primary"
      elevate-on-scroll
      scroll-target="#scrolling-techniques-7"
      app
    >
      <v-spacer></v-spacer>

      <v-menu
      offset-y
    >
      <template v-slot:activator="{ attrs, on }">
        <v-btn
          class="ma-5"
          v-bind="attrs"
          v-on="on"
        >
          {{ usuario.nome }}<v-icon>mdi-chevron-down</v-icon>
        </v-btn>
      </template>
        <v-list-item @click.stop class="white">
          <v-list-item-action class="mr-2">
            <v-switch
              v-model="isDarkMode"
              inset
              color="secondary"
            ></v-switch>
          </v-list-item-action>
          <v-list-item-title>Modo Escuro</v-list-item-title>
        </v-list-item>
    </v-menu>
    </v-app-bar>
    <v-sheet
      id="scrolling-techniques-7"
      class="overflow-y-auto"
      max-height="600"
    >
    </v-sheet>
  
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

        <v-btn
          icon
          @click.stop="mini = !mini"
          class="px-2 my-4"
        >
          <v-icon v-if="!mini">mdi-chevron-left</v-icon>
          <v-icon v-if="mini">mdi-chevron-right</v-icon>
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
      btns: [
        ['Removed', '0'],
        ['Large', 'lg'],
        ['Custom', 'b-xl'],
      ],
        colors: ['deep-purple accent-4', 'error', 'teal darken-1'],
        drawer: true,
        itemsMenu: [],
        mini: true,
        usuario: {
          nome: "Jonatas Arruda",
        },
        isDarkMode: false,
      }
      
    },
    created() {
      this.carregarItensMenu();
      const darkModePreference = localStorage.getItem('darkMode');
      if (darkModePreference !== null) {
        this.isDarkMode = JSON.parse(darkModePreference);
      } else {
        this.isDarkMode = window.matchMedia && window.matchMedia('(prefers-color-scheme: dark)').matches;
      }
      // Aplica o tema inicial
      this.$vuetify.theme.dark = this.isDarkMode;
    },
    methods: {
      carregarItensMenu() {
        const rotasDoMenu = this.$router.options.routes
          .filter(rota => rota.meta && rota.meta.showInMenu)
          .map(rota => {
            return {
              title: rota.meta.title || rota.name, 
              icon: rota.meta.icon || 'mdi-help-circle-outline', 
              path: rota.path, 
            };
          });
        this.itemsMenu = rotasDoMenu;
      }
    },
    watch: {
      isDarkMode(newVal) {
        // Atualiza o tema do Vuetify
        this.$vuetify.theme.dark = newVal;
        // Salva a preferência no localStorage
        localStorage.setItem('darkMode', JSON.stringify(newVal));
      }
    }
  }
</script>

<style scoped>
v-btn {
  background-color: black;
  color: black;
}
v-card {
  border-radius: 0px;
}
.v-list-item--active{
  color: white;
}

</style>