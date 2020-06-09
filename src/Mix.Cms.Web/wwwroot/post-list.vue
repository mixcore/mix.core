var PostPreview = Vue.component('post-preview', {
  // The todo-item component now accepts a
  // "prop", which is like a custom attribute.
  // This prop is called todo.
  props: ['post'],
  template: `<div class="post-preview">
                <a v-bind:href="post.detailsUrl">
                <h2 class="post-title">
                    {{post.title}}
                </h2>
                <h3 class="post-subtitle">
                    {{post.excerpt}}
                </h3>
                </a>
                <p class="post-meta">Posted by
                <a href="#">{{post.createdBy}}</a>
                on September 24, 2019</p>
            </div>`
})
Vue.component('post-list', { 
  props: ['pageSize', 'pageIndex', 'totalPage'], 
  data: function() {
    return {
      ver: 0,
      pIndex: 0,
      posts: []
    }
  },
  created () {
    // fetch the data when the view is created and the data is
    // already being observed
    this.pIndex = this.pageIndex;
    this.fetchData();
    this.ver +=1;
  },
  methods: {
    fetchData () {
      this.loading = true;
      // replace `getPost` with your data fetching util / API wrapper
      axios.get(`/api/v1/rest/en-us/post/portal?pageSize=${this.pageSize}&pageIndex=${this.pIndex}`)
      .then(response => {
        this.posts = this.posts.concat(response.data.items);
        console.log(this.posts, response);
        this.ver += 1;
      })
    },
    loadMore() {
        this.pIndex += 1;
        this.fetchData();
    }
  },
  components: {
    'post-preview': PostPreview
  },
  template:`<div :key="ver">
            <post-preview
                v-for="item in posts"
                :post="item"
                v-bind:key="item.id">
            </post-preview>
            <hr>
            <div v-if="totalPage > pageIndex" class="clearfix">
                <a class="btn btn-primary float-right" v-on:click="loadMore()">Older Posts &rarr;</a>
            </div></div>`

})