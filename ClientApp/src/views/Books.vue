<template>
  <div>
    <div class="order-1 sm:order-2 sm:ml-64 p-4">
      <div>
        <div>
          <div class="grid lg:grid-cols-3 md:grid-cols-1 gap-4">

<!-- First Section -->
  <Card>
    <p class="text-3xl font-semibold text-gray-900 flex justify-center pb-2">INSERT BOOKS</p>
    <form @submit.prevent="addBook">
      <div class="relative w-full h-10">
        <input
          required
          v-model="author"
          class="peer flex items-center justify-center w-full h-full bg-transparent text-blue-gray-700 font-sans font-normal outline outline-0 focus:outline-0 disabled:bg-blue-gray-50 disabled:border-0 transition-all placeholder-shown:border placeholder-shown:border-blue-gray-200 placeholder-shown:border-t-blue-gray-200 border focus:border-2 border-t-transparent focus:border-t-transparent text-sm px-3 py-2.5 rounded-[7px] border-blue-gray-200 focus:border-gray-900"
          placeholder=""
        />
        <label
          class="flex w-full h-full select-none pointer-events-none absolute left-0 font-normal !overflow-visible truncate peer-placeholder-shown:text-blue-gray-500 leading-tight peer-focus:leading-tight peer-disabled:text-transparent peer-disabled:peer-placeholder-shown:text-blue-gray-500 transition-all -top-1.5 peer-placeholder-shown:text-sm text-[11px] peer-focus:text-[11px] before:content[' '] before:block before:flex-shrink before:box-border before:w-2.5 before:h-1.5 before:mt-[6.5px] before:mr-1 peer-placeholder-shown:before:border-transparent before:rounded-tl-md before:border-t peer-focus:before:border-t-2 before:border-l peer-focus:before:border-l-2 before:pointer-events-none before:transition-all peer-disabled:before:border-transparent after:content[' '] after:block after:flex-grow after:box-border after:w-2.5 after:h-1.5 after:mt-[6.5px] after:ml-1 peer-placeholder-shown:after:border-transparent after:rounded-tr-md after:border-t peer-focus:after:border-t-2 after:border-r peer-focus:after:border-r-2 after:pointer-events-none after:transition-all peer-disabled:after:border-transparent peer-placeholder-shown:leading-[3.75] text-gray-500 peer-focus:text-gray-900 before:border-blue-gray-200 peer-focus:before:!border-gray-900 after:border-blue-gray-200 peer-focus:after:!border-gray-900"
        >
          Author
        </label>
      </div>
      <div class="relative w-full h-10 mt-2">
        <input
          required
          v-model="title"
          class="peer w-full h-full bg-transparent text-blue-gray-700 font-sans font-normal outline outline-0 focus:outline-0 disabled:bg-blue-gray-50 disabled:border-0 transition-all placeholder-shown:border placeholder-shown:border-blue-gray-200 placeholder-shown:border-t-blue-gray-200 border focus:border-2 border-t-transparent focus:border-t-transparent text-sm px-3 py-2.5 rounded-[7px] border-blue-gray-200 focus:border-gray-900"
          placeholder=""
        />
        <label
          class="flex w-full h-full select-none pointer-events-none absolute left-0 font-normal !overflow-visible truncate peer-placeholder-shown:text-blue-gray-500 leading-tight peer-focus:leading-tight peer-disabled:text-transparent peer-disabled:peer-placeholder-shown:text-blue-gray-500 transition-all -top-1.5 peer-placeholder-shown:text-sm text-[11px] peer-focus:text-[11px] before:content[' '] before:block before:flex-shrink before:box-border before:w-2.5 before:h-1.5 before:mt-[6.5px] before:mr-1 peer-placeholder-shown:before:border-transparent before:rounded-tl-md before:border-t peer-focus:before:border-t-2 before:border-l peer-focus:before:border-l-2 before:pointer-events-none before:transition-all peer-disabled:before:border-transparent after:content[' '] after:block after:flex-grow after:box-border after:w-2.5 after:h-1.5 after:mt-[6.5px] after:ml-1 peer-placeholder-shown:after:border-transparent after:rounded-tr-md after:border-t peer-focus:after:border-t-2 after:border-r peer-focus:after:border-r-2 after:pointer-events-none after:transition-all peer-disabled:after:border-transparent peer-placeholder-shown:leading-[3.75] text-gray-500 peer-focus:text-gray-900 before:border-blue-gray-200 peer-focus:before:!border-gray-900 after:border-blue-gray-200 peer-focus:after:!border-gray-900"
          :class="{ 'placeholder-shown': !title }"
        >
          Title
        </label>
      </div>
      <div class="relative inline-block w-full mt-2 text-gray-500">
        <select v-model="selectedCategory" required class="peer block appearance-none w-full border border-gray-300 hover:border-gray-400 px-4 py-2 pr-8 rounded  leading-tight focus:outline-none focus:border-blue-500">
          <option value="" disabled selected hidden>Select a Category</option>
          <option v-for="category in categories" :key="category" :value="category">{{ category.categoryName }}</option>
        </select>
        <div class="pointer-events-none absolute text-sm inset-y-0 right-0 flex items-center px-2 text-gray-700">
          <span class="flex items-center">&#9660;</span>
        </div>
      </div>
      <button type="submit" class="w-full mt-4  text-white py-2 px-4 rounded-md shadow-sm bg-gradient-to-r from-blue-500 via-blue-600 to-blue-700 hover:bg-gradient-to-br transition duration-200">Submit</button>
    </form>
  </Card>

<!-- Second Section -->
  <Card>
    <p class="text-3xl font-semibold text-gray-900 flex justify-center pb-2">SEARCH FOR BOOKS</p>
     
      <div class="relative w-full h-10 mt-2">
        <input
          required
          v-model="searchQuery"
          @input="handleInput"
          class="peer w-full h-full bg-transparent text-blue-gray-700 font-sans font-normal outline outline-0 focus:outline-0 disabled:bg-blue-gray-50 disabled:border-0 transition-all placeholder-shown:border placeholder-shown:border-blue-gray-200 placeholder-shown:border-t-blue-gray-200 border focus:border-2 border-t-transparent focus:border-t-transparent text-sm px-3 py-2.5 rounded-[7px] border-blue-gray-200 focus:border-gray-900"
          placeholder=""
        />
        <label
          class="flex w-full h-full select-none pointer-events-none absolute left-0 font-normal !overflow-visible truncate peer-placeholder-shown:text-blue-gray-500 leading-tight peer-focus:leading-tight peer-disabled:text-transparent peer-disabled:peer-placeholder-shown:text-blue-gray-500 transition-all -top-1.5 peer-placeholder-shown:text-sm text-[11px] peer-focus:text-[11px] before:content[' '] before:block before:flex-shrink before:box-border before:w-2.5 before:h-1.5 before:mt-[6.5px] before:mr-1 peer-placeholder-shown:before:border-transparent before:rounded-tl-md before:border-t peer-focus:before:border-t-2 before:border-l peer-focus:before:border-l-2 before:pointer-events-none before:transition-all peer-disabled:before:border-transparent after:content[' '] after:block after:flex-grow after:box-border after:w-2.5 after:h-1.5 after:mt-[6.5px] after:ml-1 peer-placeholder-shown:after:border-transparent after:rounded-tr-md after:border-t peer-focus:after:border-t-2 after:border-r peer-focus:after:border-r-2 after:pointer-events-none after:transition-all peer-disabled:after:border-transparent peer-placeholder-shown:leading-[3.75] text-gray-500 peer-focus:text-gray-900 before:border-blue-gray-200 peer-focus:before:!border-gray-900 after:border-blue-gray-200 peer-focus:after:!border-gray-900"
          :class="{ 'placeholder-shown': !title }"
        >
          Author
        </label>
      </div>
      <div class="relative w-full h-10 mt-2">
        <input
          v-model="searchTitle"
          @input="handleInput"
          class="peer w-full h-full bg-transparent text-blue-gray-700 font-sans font-normal outline outline-0 focus:outline-0 disabled:bg-blue-gray-50 disabled:border-0 transition-all placeholder-shown:border placeholder-shown:border-blue-gray-200 placeholder-shown:border-t-blue-gray-200 border focus:border-2 border-t-transparent focus:border-t-transparent text-sm px-3 py-2.5 rounded-[7px] border-blue-gray-200 focus:border-gray-900"
          placeholder=""
        />
        <label
          class="flex w-full h-full select-none pointer-events-none absolute left-0 font-normal !overflow-visible truncate peer-placeholder-shown:text-blue-gray-500 leading-tight peer-focus:leading-tight peer-disabled:text-transparent peer-disabled:peer-placeholder-shown:text-blue-gray-500 transition-all -top-1.5 peer-placeholder-shown:text-sm text-[11px] peer-focus:text-[11px] before:content[' '] before:block before:flex-shrink before:box-border before:w-2.5 before:h-1.5 before:mt-[6.5px] before:mr-1 peer-placeholder-shown:before:border-transparent before:rounded-tl-md before:border-t peer-focus:before:border-t-2 before:border-l peer-focus:before:border-l-2 before:pointer-events-none before:transition-all peer-disabled:before:border-transparent after:content[' '] after:block after:flex-grow after:box-border after:w-2.5 after:h-1.5 after:mt-[6.5px] after:ml-1 peer-placeholder-shown:after:border-transparent after:rounded-tr-md after:border-t peer-focus:after:border-t-2 after:border-r peer-focus:after:border-r-2 after:pointer-events-none after:transition-all peer-disabled:after:border-transparent peer-placeholder-shown:leading-[3.75] text-gray-500 peer-focus:text-gray-900 before:border-blue-gray-200 peer-focus:before:!border-gray-900 after:border-blue-gray-200 peer-focus:after:!border-gray-900"
          :class="{ 'placeholder-shown': !title }"
        >
         Title
        </label>
      </div>
  </Card>



<!-- Third Section -->
  <Card>
    <p class="text-3xl font-semibold text-gray-900 flex justify-center pb-2">SEARCH BY CATEGORY</p>
    <div class="">
      <div v-for="category in categories" :key="category.categoryId" class="mr-4 mb-2">
        <div>
          <label class="inline-flex items-center">
            <input v-model="checkedCategories" :value="category.categoryId" @change="checkFilter" class="form-checkbox text-blue-500" type="checkbox">{{ category.categoryName }}
          </label>
        </div>
      </div>
    </div>
  </Card>

</div>

        </div>

        <div v-if="loading" class="flex flex-col justify-center items-center mt-40">
          <img class="" src="../assets/loading.gif" alt="Loading animation"/>         
  <p class="text-3xl font-bold text-gray-900 flex justify-center">Your Collection Is Loading...</p>
  <p class="text-lg text-gray-600 text-center">Please wait while we retrieve your books.</p>
</div>


<div v-else>
  <div v-if="books.length > 0" class="mt-20">
    <Card>
      <div class="flex flex-wrap justify-between items-center mb-4">
        <SelectPerPage :itemsPerPage="booksPerPage" :itemsPerPageArr="booksPerPageArr" @update:itemsPerPage="handleChange" />
        
        <button @click="downloadBooks" class="mt-2 bg-gradient-to-r from-blue-500 via-blue-600 to-blue-700 hover:bg-gradient-to-br text-white py-2 px-4 rounded-md shadow-sm  w-full sm:w-auto">
          Download Collection
          <i class="fa-solid fa-download flex-shrink-0 w-5 mt-1 h-5 transition duration-75 group-hover:text-gray-900"></i>
        </button>
      </div>

      <Table
        :books="books"
        :getCategoryName="getCategoryName"
        :openModal="openModal"
        :showModal="showModal"
        :deleteBook="deleteBook"
        :closeModal="closeModal"
        :categories="categories"
        :currPage="currPage"
        :totalPages="totalPages"
        :newAuthor="newAuthor"
        :newTitle="newTitle"
        :newCategoryId="newCategoryId"
        :updateBook="updateBook"
        :openEditModal="openEditModal"
        :showEditModal="showEditModal"
        :closeEditModal="closeEditModal"
        :paginationDetails="paginationDetails"
        @update:newAuthor="(value) => (newAuthor = value)"
        @update:newTitle="(value) => (newTitle = value)"
        @update:newCategoryId="(value) => (newCategoryId = value)"
        :headers="['Author', 'Title', 'Category', 'Actions']"
      />

      <Pagination
        :currPage="currPage"
        :totalPages="totalPages"
        :visiblePages="visiblePages"
        @page-changed="changePage"
      />
    </Card>

    <DeleteModal
      :showModal="showModal"
      :closeModal="closeModal"
      :confirmDelete="deleteBook"
      :bodyText="`Are you sure you want to delete this book?`"
    />

    <EditModal
      :showEditModal="showEditModal"
      :closeEditModal="closeEditModal"
      :updateBook="updateBook"
      :categories="categories"
      :newTitle="newTitle"
      :newAuthor="newAuthor"
      :newCategoryId="newCategoryId"
      :title="currTitle"
      :author="currAuthor"
      :categoryId="currCategoryId"
      @update:newAuthor="newAuthor = $event"
      @update:newTitle="newTitle = $event"
      @update:newCategoryId="newCategoryId = $event"
    />
  </div>

<div v-else>
  <div class="flex flex-col items-center justify-center mt-40">
    <img class="" src="../assets/empty.gif" />
    <p class="text-3xl font-bold text-gray-900 flex justify-center pb-2">
      Oops! No books found.
    </p>
    <p class="text-lg text-gray-600 text-center">
      It seems your collection is empty. Start adding books to build your library!
    </p>
  </div>
</div>

</div>

     
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import axios from "axios";
import { ref, onMounted, computed, watch } from "vue";
import Card from "@/components/Card.vue";
import DeleteModal from "@/components/DeleteModal.vue";
import Table from "@/components/Table.vue";
import EditModal from "@/components/EditModal.vue";
import Pagination from "@/components/Pagination.vue";
import { url } from "@/store/authStore";
import SelectPerPage from "@/components/SelectPerPage.vue";

let debounceTimer = 0;
const author = ref<string>("");
const title = ref<string>("");
const selectedCategory = ref<{ categoryName: string; categoryId: number; } | "">("");
const loading = ref<boolean>(false);
const categories = ref<any[]>([]);
const bookCollection = ref<any[]>([]);
const bookPaginated = ref<any[]>([]);
const checkedCategories = ref<number[]>([]);
const searchQuery = ref<string>("");
const booksPerPageArr = ref<number[]>([2, 5, 10, 20, 30, 40, 50]);
const booksPerPage = ref<number>(10);
const showModal = ref<boolean>(false);
const bookIdToDelete = ref<number | null>(null);
const showEditModal = ref<boolean>(false);
const bookIdToEdit = ref<number | null>(null);
const newTitle = ref<string>("");
const newAuthor = ref<string>("");
const newCategoryId = ref<number | undefined>(0);
const books = ref<any[]>([]);
const totalFilteredBooks = ref<any[]>([]);
const currTitle = ref<string>("");
const currAuthor = ref<string>("");
const currCategoryId = ref<number | undefined>(0);
// Pagination !!!!!!!!!!!!
const currPage = ref<number>(1);
const searchTitle = ref<string>("");
const paginationDetails = ref<string>("");

const totalPages = computed(() => {
  let totalBooks;
  loading.value = true;
  if (checkedCategories.value.length > 0 || searchQuery.value || searchTitle.value) {
    totalBooks = totalFilteredBooks.value.length;
  } else {
    totalBooks = bookCollection.value?.length ?? 0;
  }
  return Math.ceil(totalBooks / booksPerPage.value);
});

const windowSize = ref<number>(3);

const visiblePages = computed(() => {
  let start = currPage.value - Math.floor(windowSize.value / 2);
  start = Math.max(start, 1);
  let end = start + windowSize.value - 1;
  end = Math.min(end, totalPages.value);
  start = Math.max(1, end - windowSize.value + 1);
  return Array.from({ length: end - start + 1 }, (_, i) => start + i);
});

const changePage = (page: number) => {
  loading.value = true;
  sessionStorage.setItem("booksPerPage", String(booksPerPage.value));
  if (page >= 1 && page <= totalPages.value) {
    currPage.value = page;
    sessionStorage.setItem("page", String(currPage.value));
    
    applyBookFilter();

  }
};

// CRUD FUNCTIONS !!!!!!!!!!!

const getBooks = async () => {
  loading.value = true;
  axios
    .get(`${url}api/getbooks`, {
      params: {
        pageNumber: currPage.value,
        pageSize: booksPerPage.value,
      },
      withCredentials: true,
    })
    .then((response) => {
      bookPaginated.value = response.data.books;
      books.value = response.data.books;
      paginationDetails.value = response.data.range;
    })
    .catch((error) => {
      console.error(error);
    });
};

const filterBooksAll = async () => {

  loading.value = true;
  const searchParams = {
    search: searchQuery.value,
    searchTitle: searchTitle.value,
    categories: checkedCategories.value.join(","),
  };
 await axios
    .get(`${url}api/searchandcategoryall`, {
      params: searchParams,
      withCredentials: true,
    })
    .then((response) => {
      totalFilteredBooks.value = response.data;
    })
    .catch((error) => {
      console.error(error);
    }).finally(async()=>{
      //loading.value = false;
    await  filterBooksPaginated();
    })
};

const filterBooksPaginated = async () => {
  //loading.value = true;
  paginationDetails.value = "";
  const searchParams = {
    search: searchQuery.value,
    searchTitle: searchTitle.value,
    categories: checkedCategories.value.join(","),
    pageNumber: currPage.value,
    pageSize: booksPerPage.value,
  };
 await axios
    .get(`${url}api/searchandcategory`, {
      params: searchParams,
      withCredentials: true,
    })
    .then((response) => {
      books.value = response.data.books; 
      paginationDetails.value = response.data.range;     
      sessionStorage.setItem("search", searchQuery.value);
      sessionStorage.setItem("categories", String(checkedCategories.value));
      sessionStorage.setItem("page", String(currPage.value));
      sessionStorage.setItem("title", searchTitle.value);

    })
    .catch((error) => {
      console.error(error);
    }).finally(()=>{
      loading.value = false;
    })
};

async function checkFilter() {
  //loading.value=true;
  if(checkedCategories.value.length > 0 && !searchQuery.value && !searchTitle.value){
       await filterBooksAll();
      } else if (checkedCategories.value.length === 0 && searchQuery.value || searchTitle.value) {
         
       await handleInput();
      }else if (checkedCategories.value.length > 0 && searchQuery.value || searchTitle.value) {
      await  handleInput();
      }
      else {
        await getBooks();
        await getAllBooks();
      }
      sessionStorage.setItem("categories", String(checkedCategories.value));
}

async function applyBookFilter() {
  if (checkedCategories.value.length > 0 || searchQuery.value || searchTitle.value) {
     await filterBooksAll();
    } else {
     await getBooks();
    await  getAllBooks();
    }
}

const getAllBooks = async () => {
  loading.value = true;
  axios
    .get(`${url}api/getallbooks`, {
      withCredentials: true,
    })
    .then((response) => {
      bookCollection.value = response.data;
    })
    .catch((error) => {
      console.error(error);
    }).finally(()=>{
      loading.value = false;
    })
};

const getAllCategories = async () => {
 await axios
    .get(`${url}api/categories`, { withCredentials: true })
    .then((response) => {
      categories.value = response.data;
    });
};

const addBook = () => {
  loading.value=true;
  const categoryIdToAdd = selectedCategory.value
    ? selectedCategory.value.categoryId
    : null;
  axios
    .post(
      `${url}api/insertbook`,
      {
        author: author.value,
        title: title.value,
        CategoryId: categoryIdToAdd,
      },
      { withCredentials: true }
    )
    .then((response) => {
      author.value = "";
      title.value = "";
      selectedCategory.value = "";

      applyBookFilter();

      
    })
    .catch((error) => {
      console.error(error);
    });
};

const deleteBook = async () => {
  loading.value=true;
  showModal.value = false;
  document.body.style.overflow = "auto";
 await axios
    .delete(`${url}api/deletebook/${bookIdToDelete.value}`, {
      withCredentials: true,
    })
    .then(() => {
      const totalBooks = totalFilteredBooks.value.length;
      const totalPagesToDelete = Math.ceil(totalBooks / booksPerPage.value);
      if (totalPagesToDelete < currPage.value) {
        currPage.value - 1;
      }
      applyBookFilter();



    })
    .catch((error) => {
      console.error(
        `Error deleting book with ID ${bookIdToDelete.value}:`,
        error
      );
    });
};

// Modal functions !!!!!!!!!
const openEditModal = (book: any) => {
  document.body.style.overflow = "hidden";
  showEditModal.value = !showEditModal.value;
  bookIdToEdit.value = book.bookId;
  currTitle.value = book.title;
  currAuthor.value = book.author;
  currCategoryId.value = book.categoryId;
};

const updateBook = () => {
  axios
    .post(
      `${url}api/updatebook`,
      {
        Id: bookIdToEdit.value,
        NewTitle: newTitle.value,
        NewAuthor: newAuthor.value,
        NewCategory: newCategoryId.value,
      },
      { withCredentials: true }
    )
    .then((response) => {
      applyBookFilter();
      document.body.style.overflow = "auto";
      showEditModal.value = false;
      bookIdToEdit.value = null;
      newTitle.value = "";
      newAuthor.value = "";
      newCategoryId.value = 0;
    })
    .catch((error) => {
      console.error(error); // Log any errors
    });
};

function closeEditModal() {
  document.body.style.overflow = "auto";
  showEditModal.value = false;
  bookIdToEdit.value = null;
  newTitle.value = "";
  newAuthor.value = "";
  newCategoryId.value = 0;
}

const openModal = (bookId: number) => {
  document.body.style.overflow = "hidden";
  showModal.value = !showModal.value;
  bookIdToDelete.value = bookId;
};

const closeModal = () => {
  document.body.style.overflow = "auto";
  showModal.value = false;
};

// Handle fucntions !!!!!!!!!!!!

function handleChange(newValue: number) {
  loading.value = true;
  booksPerPage.value = newValue;
  currPage.value = 1;
  sessionStorage.setItem("booksPerPage", String(booksPerPage.value));
  sessionStorage.setItem("page", String(currPage.value));

  applyBookFilter();
}

const handleInput = async () => {
  //currPage.value = 1;
  loading.value = true;
 // books.value=[];
  clearTimeout(debounceTimer);
  sessionStorage.removeItem("search");
  sessionStorage.removeItem("title");
  if (searchQuery.value.trim() !== "" || searchTitle.value.trim() !== "") {
    debounceTimer = setTimeout(async () => {
     // loading.value = true;
     await filterBooksAll();
      //loading.value = false;
    }, 1000);
  }else{
    await getBooks();
   await getAllBooks();
  }
};

const getCategoryName = (categoryId: number) => {
  const category = categories.value.find(
    (cat) => cat.categoryId === categoryId
  );
  return category ? category.categoryName : "Unknown Category";
};

// Watchers !!!!!!!!!!!!!!!!!!!!
watch([checkedCategories, searchQuery, searchTitle], () => {
  const isReload = sessionStorage.getItem("isReload");

  if (isReload) {
    sessionStorage.removeItem("isReload");
    return;
  }

  currPage.value = 1;

  checkFilter();
});

window.onbeforeunload = function () {
  sessionStorage.setItem("isReload", "true");
};

watch(
  () => newTitle.value,
  (newVal) => {
    currTitle.value = newVal;
  }
);

watch(
  () => newAuthor.value,
  (newVal) => {
    currAuthor.value = newVal;
  }
);

watch(
  () => newCategoryId.value,
  (newVal) => {
    currCategoryId.value = newVal;
  }
);

watch([totalPages, currPage], async () => {
  loading.value=true;
  if (currPage.value > totalPages.value) {
    currPage.value = Math.max(totalPages.value, 1);
    sessionStorage.setItem("page", String(currPage.value));
  //   books.value = [];
  //  bookPaginated.value = [];
 if(!checkedCategories.value.length && !searchQuery.value && !searchTitle.value){
   await getBooks();
   await getAllBooks();
  }else{
    await checkFilter();
  }
   


  

  loading.value=true;
  }
});

// Lifecycle Hook
onMounted(async()  => {
  await getAllCategories();
  const categoriesFromStorage = sessionStorage.getItem("categories");
  const searchFromStorage = sessionStorage.getItem("search");
  const pageFromStorage = sessionStorage.getItem("page");
  const booksPerPageFromStorage = sessionStorage.getItem("booksPerPage");
  const bookTitleFromStorage = sessionStorage.getItem("title");

  if (booksPerPageFromStorage) {
    booksPerPage.value = parseInt(booksPerPageFromStorage, 10);
  }

  if (pageFromStorage) {
    currPage.value = parseInt(pageFromStorage, 10);
  }

  if (categoriesFromStorage || searchFromStorage || pageFromStorage) {
     checkedCategories.value = categoriesFromStorage ? categoriesFromStorage.split(",").map(Number) : [];
     searchQuery.value= searchFromStorage ? searchFromStorage : "";

      searchTitle.value= bookTitleFromStorage ? bookTitleFromStorage : "";
 
   await checkFilter();
  }
else {
   await getBooks();
    await getAllBooks();
  }
});
const downloadBooks = () => {
  axios
    .get(`${url}api/downloadbooks`, { responseType: "arraybuffer", withCredentials: true })
    .then((response) => {
      const blob = new Blob([response.data], {
        type: response.headers["content-type"],
      });
      const link = document.createElement("a");
      link.href = window.URL.createObjectURL(blob);
      link.download = "books.txt";
      link.click();
    })
    .catch((error) => {
      console.error(error);
    });
};
</script>