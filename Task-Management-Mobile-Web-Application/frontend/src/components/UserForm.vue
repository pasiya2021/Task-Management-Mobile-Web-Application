<template>
  <transition name="slide-over">
    <div class="fixed inset-0 z-50 flex">
      <!-- Overlay -->
      <div class="fixed inset-0 bg-black bg-opacity-30 transition-opacity" @click="closeModal"></div>
      <!-- Panel -->
      <div class="ml-auto w-full max-w-md h-full bg-white shadow-xl flex flex-col relative animate-slidein">
        <button @click="closeModal" class="absolute top-4 right-4 text-teal-700 hover:text-amber-500 focus:outline-none">
          <svg class="w-7 h-7" fill="none" stroke="currentColor" viewBox="0 0 24 24">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12" />
          </svg>
        </button>
        <div class="p-8">
          <h3 class="text-2xl font-bold text-teal-700 mb-6">Create New User</h3>
          <form @submit.prevent="handleSubmit" class="space-y-6">
            <div>
              <label for="name" class="block text-sm font-medium text-teal-700 mb-1">
                Name *
              </label>
              <input
                id="name"
                v-model="form.name"
                type="text"
                required
                class="w-full px-3 py-2 border border-teal-300 rounded-lg shadow-sm focus:outline-none focus:ring-2 focus:ring-amber-400 focus:border-amber-400"
                :class="{ 'border-red-500': errors.name }"
              />
              <p v-if="errors.name" class="mt-1 text-sm text-red-600">{{ errors.name }}</p>
            </div>
            <div>
              <label for="email" class="block text-sm font-medium text-teal-700 mb-1">
                Email *
              </label>
              <input
                id="email"
                v-model="form.email"
                type="email"
                required
                class="w-full px-3 py-2 border border-teal-300 rounded-lg shadow-sm focus:outline-none focus:ring-2 focus:ring-amber-400 focus:border-amber-400"
                :class="{ 'border-red-500': errors.email }"
              />
              <p v-if="errors.email" class="mt-1 text-sm text-red-600">{{ errors.email }}</p>
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
                {{ submitting ? 'Creating...' : 'Create User' }}
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
import { ref, reactive } from 'vue';
import type { CreateUserRequest } from '../types';

const emit = defineEmits<{
  close: [];
  submit: [data: CreateUserRequest];
}>();

const submitting = ref(false);

const form = reactive({
  name: '',
  email: ''
});

const errors = reactive({
  name: '',
  email: ''
});

const validateForm = () => {
  errors.name = '';
  errors.email = '';
  
  let isValid = true;
  
  if (!form.name.trim()) {
    errors.name = 'Name is required';
    isValid = false;
  }
  
  if (!form.email.trim()) {
    errors.email = 'Email is required';
    isValid = false;
  } else if (!/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(form.email)) {
    errors.email = 'Please enter a valid email address';
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
    const response = await fetch('http://192.168.64.77:7000/api/users', {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({
        name: form.name.trim(),
        email: form.email.trim()
      })
    });
    if (!response.ok) {
      throw new Error('Failed to create user');
    }
    alert('User created successfully');
    closeModal();
  } catch (e) {
    alert('Error creating user');
  } finally {
    submitting.value = false;
  }
};

const closeModal = () => {
  emit('close');
};
</script>