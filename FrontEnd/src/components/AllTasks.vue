<script setup>
  import { ref, onMounted } from 'vue';
  import axios from 'axios';
  import TaskComponent from '@/components/Task.vue';
  import AddTaskModal from '@/components/AddTaskModal.vue';
  import EditTaskModal from '@/components/EditTaskModal.vue'; // Import the edit modal
  import dayjs from 'dayjs';
  import { useRoute } from 'vue-router';
  import { useToast } from 'vue-toastification';

  // Get userId from route params
  const userId = Number(localStorage.getItem('userId'));
  const toast = useToast();

  // Task data (initialized empty, will be fetched from API)
  const tasks = ref([]);
  const overdueTasks = ref([]);
  const todayTasks = ref([]);
  const upcomingTasks = ref([]);

  // Modal visibility and current task details
  const isAddModalVisible = ref(false);
  const isEditModalVisible = ref(false);
  const currentTaskType = ref('');
  const currentTask = ref(null); // Task being edited

  // Fetch tasks from API
  const fetchTasks = async () => {
    try {
      const response = await axios.get(`http://localhost:5059/api/TaskAssignment/user/${userId}`);
      tasks.value = Array.isArray(response.data) ? response.data : [];
      console.log(tasks.value);
      updateTaskLists();
    } catch (error) {
      console.error('Error fetching tasks:', error.message);
      tasks.value = [];
    }
  };

  // Categorize tasks based on due date
  const updateTaskLists = () => {
    // Overdue tasks: Status is 'completed' and due date is before today
    overdueTasks.value = tasks.value.filter(task =>
      task.status === 'completed' || task.status === 'overdue' || dayjs(task.dueDate).isBefore(dayjs(), 'day')
    );

    // Today tasks: Tasks with due date equal to today
    todayTasks.value = tasks.value.filter(task =>
      task.status === 'pending' && dayjs(task.dueDate).isSame(dayjs(), 'day')
    );

    // Upcoming tasks: Tasks with due date after today
    upcomingTasks.value = tasks.value.filter(task =>
      task.status === 'pending' && dayjs(task.dueDate).isAfter(dayjs(), 'day')
    );
  };

  // Open modals
  const openAddModal = (taskType) => {
    currentTaskType.value = taskType;
    isAddModalVisible.value = true;
  };

  const openEditModal = (task) => {
    currentTask.value = task;
    isEditModalVisible.value = true;
  };

  // Update task after editing
  const updateTask = (updatedTask) => {
    const index = tasks.value.findIndex(task => task.id === updatedTask.id);
    if (index !== -1) {
      tasks.value[index] = updatedTask;
      updateTaskLists();
      toast.success('Update the task successfully');
    }
    isEditModalVisible.value = false;
  };

  // Add new task
  const addNewTask = (newTask) => {
    newTask.userId = Number(userId);
    tasks.value.push(newTask);
    updateTaskLists();
    isAddModalVisible.value = false;
    toast.success('Add a new task successfully!!');
  };

  // Close modals
  const closeAddModal = () => {
    isAddModalVisible.value = false;
  };

  const closeEditModal = () => {
    isEditModalVisible.value = false;
  };

  // Remove task
  const removeTask = (taskId) => {
    tasks.value = tasks.value.filter(task => task.id !== taskId);
    updateTaskLists();
    toast.success('Remove the task successfully!!');
  };

  // Fetch tasks when the component is mounted
  onMounted(fetchTasks);
</script>


<template>
  <div class="p-6 bg-gray-100 min-h-screen flex w-full">
    <h1 class="sr-only">Kanban Board</h1>
    <div class="flex w-full space-x-4">
      <!-- Overdue Tasks Column -->
      <div class="flex-1 bg-white shadow-md rounded-lg p-4">
        <h2 class="text-lg font-semibold mb-2 text-red-500">Overdue Tasks</h2>
        <div class="space-y-2">
          <TaskComponent v-for="task in overdueTasks" :key="task.id" :task="task" @remove-task="removeTask"
            @edit-task="openEditModal" />
        </div>
      </div>

      <!-- Today's Tasks Column -->
      <div class="flex-1 bg-white shadow-md rounded-lg p-4">
        <div class="flex justify-between items-center">
          <h2 class="text-lg font-semibold mb-2 text-blue-500">Today's Tasks</h2>
          <button @click="openAddModal('today')"
            class="bg-blue-500 text-white rounded-full w-7 h-7 flex items-center justify-center hover:bg-blue-600 focus:outline-none shadow-lg transform transition-all duration-300 ease-in-out hover:scale-110">
            <span class="text-xl font-bold">+</span>
          </button>
        </div>
        <div class="space-y-2">
          <TaskComponent v-for="task in todayTasks" :key="task.id" :task="task" @remove-task="removeTask"
            @edit-task="openEditModal" />
        </div>
      </div>

      <!-- Upcoming Tasks Column -->
      <div class="flex-1 bg-white shadow-md rounded-lg p-4">
        <div class="flex justify-between items-center">
          <h2 class="text-lg font-semibold mb-2 text-green-500">Upcoming Tasks</h2>
          <button @click="openAddModal('upcoming')"
            class="bg-green-500 text-white rounded-full w-7 h-7 flex items-center justify-center hover:bg-green-600 focus:outline-none shadow-lg transform transition-all duration-300 ease-in-out hover:scale-110">
            <span class="text-xl font-bold">+</span>
          </button>
        </div>
        <div class="space-y-2">
          <TaskComponent v-for="task in upcomingTasks" :key="task.id" :task="task" @remove-task="removeTask"
            @edit-task="openEditModal" />
        </div>
      </div>
    </div>

    <!-- Add Task Modal -->
    <AddTaskModal v-if="isAddModalVisible" :userId="userId" @add-task="addNewTask" @close-modal="closeAddModal" />

    <!-- Edit Task Modal -->
    <EditTaskModal v-if="isEditModalVisible" :taskData="currentTask" @update-task="updateTask"
      @close-modal="closeEditModal" />
  </div>
</template>
