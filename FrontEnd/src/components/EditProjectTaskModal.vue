<script setup>
  import { ref, onMounted } from 'vue';
  import axios from 'axios';

  const props = defineProps({
    taskData: {
      type: Object,
      required: true
    }
  });

  const emits = defineEmits(['update-task', 'close-modal']);

  const task = ref({ ...props.taskData });

  const priorities = ref([]);

  // Fetching priorities from the API
  const fetchPriorities = async () => {
    try {
      const response = await axios.get('http://localhost:5059/api/Priority');
      priorities.value = response.data;
    } catch (error) {
      console.error('Error fetching priorities:', error);
      alert('Failed to load priorities. Please try again later.');
    }
  };

  // Fetch priorities when the component is mounted
  onMounted(() => {
    fetchPriorities();
  });

  // Submit the updated task
  const submitTask = async () => {
    try {
      // Only send the fields that need to be updated
      const updatedTaskPayload = {
        title: task.value.title,
        description: task.value.description,
        status: task.value.status,
        dueDate: task.value.dueDate,
        priorityId: task.value.priorityId
      };

      console.log('Submitting task:', updatedTaskPayload); // Log the task data being submitted

      const response = await axios.put(`http://localhost:5059/api/Task/update/${task.value.id}`, updatedTaskPayload);
      emits('update-task', response.data);
      closeModal();
    } catch (error) {
      console.error('Error updating task:', error.response ? error.response.data : error.message);
      alert('Failed to update task. Please try again.');
    }
  };

  // Close the modal
  const closeModal = () => {
    emits('close-modal');
  };
</script>
<template>
  <div class="fixed inset-0 bg-gray-800 bg-opacity-50 flex justify-center items-center z-50">
    <div class="bg-white p-6 rounded-lg shadow-lg w-96">
      <h2 class="text-lg font-bold mb-4">Edit The Task</h2>
      <form @submit.prevent="submitTask">
        <!-- Task Title -->
        <div class="mb-4">
          <label for="title" class="block text-sm font-medium text-gray-700">Title</label>
          <input v-model="task.title" type="text" id="title"
            class="mt-1 block w-full border-gray-300 rounded-md shadow-sm focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm p-1"
            placeholder="Enter task title" required />
        </div>

        <!-- Description -->
        <div class="mb-4">
          <label for="description" class="block text-sm font-medium text-gray-700">Description</label>
          <textarea v-model="task.description" id="description" rows="3"
            class="mt-1 block w-full border-gray-300 rounded-md shadow-sm focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm p-1"
            placeholder="Enter task description"></textarea>
        </div>

        <!-- Status -->
        <div class="mb-4">
          <label for="status" class="block text-sm font-medium text-gray-700">Status</label>
          <select v-model="task.status" id="status"
            class="mt-1 block w-full border-gray-300 rounded-md shadow-sm focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm p-1"
            required>
            <option value="pending">Pending</option>
            <option value="completed">Completed</option>
          </select>
        </div>

        <!-- Due Date -->
        <div class="mb-4">
          <label for="dueDate" class="block text-sm font-medium text-gray-700">Due Date</label>
          <input v-model="task.dueDate" type="date" id="dueDate"
            class="mt-1 block w-full border-gray-300 rounded-md shadow-sm focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm p-1"
            required />
        </div>

        <!-- Priority -->
        <div class="mb-4">
          <label for="priority" class="block text-sm font-medium text-gray-700">Priority</label>
          <select v-model="task.priorityId" id="priority"
            class="mt-1 block w-full border-gray-300 rounded-md shadow-sm focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm p-1"
            required>
            <option value="0" disabled>Select priority</option>
            <option v-for="priority in priorities" :key="priority.id" :value="priority.id">
              {{ priority.title }}
            </option>
          </select>
        </div>

        <div class="flex justify-end space-x-2">
          <button type="button" @click="closeModal"
            class="px-4 py-2 bg-gray-200 rounded-md text-gray-700 hover:bg-gray-300">
            Cancel
          </button>
          <button type="submit" class="px-4 py-2 bg-blue-500 text-white rounded-md hover:bg-blue-600">
            Confirm
          </button>
        </div>
      </form>
    </div>
  </div>
</template>
