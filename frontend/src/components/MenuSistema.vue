<template>
<div class="fill-height" fluid>
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
      <v-list-item class="px-2" dark>

        
        <v-list-item-title v-show="!mini">John Leider</v-list-item-title>

        <v-btn
          icon
          @click.stop="mini = !mini"
        >
          <v-icon>mdi-chevron-left</v-icon>
        </v-btn>
      </v-list-item>


      <v-list dense>
        <v-list-item
          v-for="item in items"
          :key="item.title"
          link
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
    data () {
      return {
        drawer: true,
        items: [
          { title: 'Home', icon: 'mdi-home-city' },
          { title: 'My Account', icon: 'mdi-account' },
          { title: 'Users', icon: 'mdi-account-group-outline' },
        ],
        mini: true,
        termoBusca: '',
      }
    },
    computed: {
            itensFiltrados() {
                if (!this.termoBusca) {
                return this.items;
            }
            const busca = this.termoBusca.toLowerCase();
            return this.items.filter(item => item.title.toLowerCase().includes(busca));
            }
        }
    }
</script>