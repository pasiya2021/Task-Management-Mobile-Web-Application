<template>
  <aside class="h-full w-60 bg-gradient-to-b from-teal-600 to-amber-400 text-white flex flex-col p-0 shadow-xl">
    <div class="flex items-center gap-2 px-6 py-6 border-b border-amber-300">
      <svg class="w-7 h-7 text-amber-300" fill="none" stroke="currentColor" viewBox="0 0 24 24">
        <circle cx="12" cy="12" r="10" stroke-width="2" />
        <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 14c-2.5 0-4-1.5-4-4s1.5-4 4-4 4 1.5 4 4-1.5 4-4 4zm0 2c2.67 0 8 1.34 8 4v2H4v-2c0-2.66 5.33-4 8-4z" />
      </svg>
      <span class="font-bold text-lg tracking-wide">Users</span>
    </div>
    <nav class="flex-1 overflow-y-auto px-2 py-4">
      <ul class="space-y-1">
        <li>
          <button
            class="w-full flex items-center gap-2 px-4 py-2 rounded-lg hover:bg-amber-200 hover:text-teal-900 transition font-semibold"
            :class="{ 'bg-white text-teal-700': selectedUserId === '' }"
            @click="selectUser('')"
          >
            <svg class="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M17 20h5v-2a4 4 0 00-3-3.87M9 20H4v-2a4 4 0 013-3.87m7-6.13a4 4 0 11-8 0 4 4 0 018 0z" />
            </svg>
            All Users
          </button>
        </li>
        <li v-for="user in users" :key="user.id">
          <div class="flex items-center w-full mb-2">
            <button
              :class="['flex-1 flex items-center px-4 py-2 rounded-lg transition',
                selectedUserId === user.id
                  ? 'bg-gradient-to-r from-teal-400 to-amber-300 text-white shadow-lg'
                  : 'bg-white text-gray-700 hover:bg-teal-50']"
              @click="selectUser(user.id)"
            >
              <span class="mr-2">
                <svg v-if="selectedUserId === user.id" class="w-5 h-5 text-amber-500" fill="currentColor" viewBox="0 0 20 20">
                  <path d="M10 2a8 8 0 100 16 8 8 0 000-16zm0 14a6 6 0 110-12 6 6 0 010 12z" />
                </svg>
                <svg v-else class="w-5 h-5 text-teal-400" fill="currentColor" viewBox="0 0 20 20">
                  <path d="M10 2a8 8 0 100 16 8 8 0 000-16zm0 14a6 6 0 110-12 6 6 0 010 12z" />
                </svg>
              </span>
              <span class="flex-1 text-left truncate">{{ user.name }}</span>
            </button>
            <button @click="deleteUser(user.id)" class="ml-2 text-red-500 hover:text-red-700 focus:outline-none" title="Delete user">
              <svg class="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12" />
              </svg>
            </button>
          </div>
        </li>
      </ul>
    </nav>
  </aside>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue';
import type { User } from '../types';
import { userService } from '../services/api';

const users = ref<User[]>([]);
const selectedUserId = ref<number | ''>('');

const emit = defineEmits<{
  userChanged: [userId: number | '']
}>();

onMounted(async () => {
  try {
    users.value = await userService.getAllUsers();
  } catch (error) {
    console.error('Failed to load users:', error);
  }
});
function deleteUser(id) {
  if (!confirm('Are you sure you want to delete this user?')) return;
  fetch(`http://192.168.64.77:7000/api/users/${id}`, {
    method: 'DELETE'
  })
    .then(res => {
      if (!res.ok) throw new Error('Failed');
      alert('User deleted');
      loadUsers();
    })
    .catch(() => alert('Error deleting user'));
}
</script>