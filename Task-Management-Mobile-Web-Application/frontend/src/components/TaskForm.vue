<template>
  <transition name="slide-over">
    <div class="fixed inset-0 z-50 flex">
      <!-- Overlay -->
      <div class="fixed inset-0 bg-black bg-opacity-30 transition-opacity" @click="closeModal"></div>
      <!-- Panel -->
      <div class="ml-auto w-full max-w-xl h-full bg-white shadow-xl flex flex-col relative animate-slidein">
        <button @click="closeModal" class="absolute top-4 right-4 text-teal-700 hover:text-amber-500 focus:outline-none">
          <svg class="w-7 h-7" fill="none" stroke="currentColor" viewBox="0 0 24 24">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12" />
          </svg>
        </button>
        <div class="p-8">
          <h3 class="text-2xl font-bold text-teal-700 mb-6">
            {{ isEditing ? 'Edit Task' : 'Create New Task' }}
          </h3>
          <form @submit.prevent="handleSubmit" class="space-y-6">
            <div>
              <label for="title" class="block text-sm font-medium text-teal-700 mb-1">
                Title *
              </label>
              <input
                id="title"
                v-model="form.title"
                type="text"
                required
                class="w-full px-3 py-2 border border-teal-300 rounded-lg shadow-sm focus:outline-none focus:ring-2 focus:ring-amber-400 focus:border-amber-400"
                :class="{ 'border-red-500': errors.title }"
              />
              <p v-if="errors.title" class="mt-1 text-sm text-red-600">{{ errors.title }}</p>
            </div>
            <div>
              <label for="description" class="block text-sm font-medium text-teal-700 mb-1">
                Description
              </label>
              <textarea
                id="description"
                v-model="form.description"
                rows="3"
                class="w-full px-3 py-2 border border-teal-300 rounded-lg shadow-sm focus:outline-none focus:ring-2 focus:ring-amber-400 focus:border-amber-400"
              ></textarea>
            </div>
            <div>
              <label for="status" class="block text-sm font-medium text-teal-700 mb-1">
                Status
              </label>
              <select
                id="status"
                v-model="form.status"
                class="w-full px-3 py-2 border border-teal-300 rounded-lg shadow-sm focus:outline-none focus:ring-2 focus:ring-amber-400 focus:border-amber-400"
              >
                <option value="Pending">Pending</option>
                <option value="In Progress">In Progress</option>
                <option value="Completed">Completed</option>
              </select>
            </div>
            <div v-if="!isEditing">
              <label for="userId" class="block text-sm font-medium text-teal-700 mb-1">
                Assign to User *
              </label>
              <select
                id="userId"
                v-model="form.userId"
                required
                class="w-full px-3 py-2 border border-teal-300 rounded-lg shadow-sm focus:outline-none focus:ring-2 focus:ring-amber-400 focus:border-amber-400"
                :class="{ 'border-red-500': errors.userId }"
              >
                <option value="">Select a user</option>
                <option v-for="user in users" :key="user.id" :value="user.id">
                  {{ user.name }}
                </option>
              </select>
              <p v-if="errors.userId" class="mt-1 text-sm text-red-600">{{ errors.userId }}</p>
            </div>
            <div>
              <label for="dueDate" class="block text-sm font-medium text-teal-700 mb-1">
                Due Date
              </label>
              <input
                id="dueDate"
                v-model="form.dueDate"
                type="date"
                class="w-full px-3 py-2 border border-teal-300 rounded-lg shadow-sm focus:outline-none focus:ring-2 focus:ring-amber-400 focus:border-amber-400"
              />
            </div>
            <div class="flex justify-end space-x-3 pt-4">
              <button
                type="button"
                @click="closeModal"
                class="px-4 py-2 text-sm font-medium text-teal-700 bg-amber-100 border border-amber-200 rounded-lg hover:bg-amber-200 focus:outline-none focus:ring-2 focus:ring-amber-400"
              >
                Cancel
              </button>
              <button
                type="submit"
                :disabled="submitting"
                class="px-4 py-2 text-sm font-medium text-white bg-teal-600 border border-transparent rounded-lg hover:bg-teal-700 focus:outline-none focus:ring-2 focus:ring-teal-400 disabled:opacity-50 disabled:cursor-not-allowed"
              >
                {{ submitting ? 'Saving...' : (isEditing ? 'Update' : 'Create') }}
              </button>
            </div>
          </form>
        </div>
      </div>
    </div>
  </transition>
</template>

<style scoped>
.slide-over-enter-active,
.slide-over-leave-active {
  transition: transform 0.3s cubic-bezier(0.4, 0, 0.2, 1);
}
.slide-over-enter-from,
.slide-over-leave-to {
  transform: translateX(100%);
}
.slide-over-enter-to,
.slide-over-leave-from {
  transform: translateX(0);
}
</style>

<script setup lang="ts">
import { ref, reactive, onMounted, watch } from 'vue';
import type { User, Task, CreateTaskRequest, UpdateTaskRequest } from '../types';
import { userService } from '../services/api';

const props = defineProps<{
  task?: Task;
}>();

const emit = defineEmits<{
  close: [];
  submit: [data: CreateTaskRequest | UpdateTaskRequest];
}>();

const users = ref<User[]>([]);
const submitting = ref(false);
const isEditing = ref(!!props.task);

const form = reactive({
  title: '',
  description: '',
  status: 'Pending' as 'Pending' | 'In Progress' | 'Completed',
  userId: 0,
  dueDate: ''
});

const errors = reactive({
  title: '',
  userId: ''
});

// Load users on mount
onMounted(async () => {
  try {
    users.value = await userService.getAllUsers();
  } catch (error) {
    console.error('Failed to load users:', error);
  }
});

// Populate form if editing
watch(() => props.task, (task) => {
  if (task) {
    form.title = task.title;
    form.description = task.description;
    form.status = task.status;
    form.userId = task.userId;
    form.dueDate = task.dueDate ? task.dueDate.split('T')[0] : '';
    isEditing.value = true;
  }
}, { immediate: true });

const validateForm = () => {
  errors.title = '';
  errors.userId = '';
  
  let isValid = true;
  
  if (!form.title.trim()) {
    errors.title = 'Title is required';
    isValid = false;
  }
  
  if (!isEditing.value && !form.userId) {
    errors.userId = 'Please select a user';
    isValid = false;
  }
  
  return isValid;
};

const handleSubmit = async () => {
  if (!validateForm()) {
    return;
  }
  submitting.value = true;
  try {
    let url = '';
    let method = '';
    let body = {};
    if (isEditing.value && props.task) {
      url = `http://192.168.64.77:7000/api/tasks/${props.task.id}`;
      method = 'PUT';
      body = {
        title: form.title.trim(),
        description: form.description.trim(),
        status: form.status,
        dueDate: form.dueDate
      };
    } else {
      url = 'http://192.168.64.77:7000/api/tasks';
      method = 'POST';
      body = {
        title: form.title.trim(),
        description: form.description.trim(),
        status: form.status,
        userId: form.userId,
        dueDate: form.dueDate
      };
    }
    const response = await fetch(url, {
      method,
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(body)
    });
    if (!response.ok) {
      throw new Error('Failed to save task');
    }
    alert(isEditing.value ? 'Task updated successfully' : 'Task created successfully');
    closeModal();
  } catch (e) {
    alert('Error saving task');
  } finally {
    submitting.value = false;
  }
};

const closeModal = () => {
  emit('close');
};
</script>