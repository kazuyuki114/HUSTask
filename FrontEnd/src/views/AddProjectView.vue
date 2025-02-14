<script setup>
    import { ref, onMounted } from 'vue';
    import axios from 'axios';
    import { useRouter } from 'vue-router';
    import UserCard from '@/components/UserCard.vue';
    import postLog from '@/logs/postLog';

    const router = useRouter();
    const currentUserId = Number(localStorage.getItem('userId')); // Current user ID
    // Project details
    const name = ref('');
    const description = ref('');
    const startDate = ref('');
    const endDate = ref('');
    const status = ref('');
    const selectedUsers = ref([]); // Assigned members

    // Error and success messages
    const errorMessage = ref('');
    const successMessage = ref('');

    // Modal visibility and search query
    const isModalOpen = ref(false);
    const searchQuery = ref('');

    // Users data
    const users = ref([]);
    const filteredUsers = ref([]);

    // Fetch users excluding current user
    const fetchUsers = async () => {
        try {
            const response = await axios.get('http://localhost:5059/api/users');
            users.value = response.data.filter(
                (user) => user.id !== parseInt(currentUserId)
            );
            recomputeFilteredUsers(); // Initialize filtered users
        } catch (error) {
            errorMessage.value = 'Failed to load users.';
        }
    };

    // Filter users based on search and exclude already assigned members
    const recomputeFilteredUsers = () => {
        const query = searchQuery.value.toLowerCase();
        filteredUsers.value = users.value.filter(
            (user) =>
                !selectedUsers.value.find((selected) => selected.id === user.id) &&
                (user.email.toLowerCase().includes(query) ||
                    user.name.toLowerCase().includes(query))
        );
    };

    // Add user to the project
    const addUserToProject = (user) => {
        if (!selectedUsers.value.find((selected) => selected.id === user.id)) {
            selectedUsers.value.push(user);
            recomputeFilteredUsers(); // Update modal list
        }
    };

    // Remove user from the project
    const removeUserFromProject = (userId) => {
        selectedUsers.value = selectedUsers.value.filter((user) => user.id !== userId);
        recomputeFilteredUsers(); // Update modal list
    };

    // Open modal and recompute the user list
    const openModal = () => {
        searchQuery.value = ''; // Clear search
        recomputeFilteredUsers(); // Refresh modal list
        isModalOpen.value = true;
    };

    // Close modal
    const closeModal = () => {
        isModalOpen.value = false;
    };

    // Submit form to add project
    // Submit form to add project
    const addProject = async () => {
        const projectData = {
            name: name.value,
            description: description.value,
            startDate: startDate.value,
            endDate: endDate.value,
            status: status.value,
        };
        try {

            // Step 1: Add the project
            const response = await axios.post('http://localhost:5059/api/project', projectData);
            const projectId = response.data.id; // Assume the response contains the project ID

            // Step 2: Add current user as the leader
            const leaderData = {
                userId: currentUserId,
                projectId: projectId,
                role: 'Leader',
            };
            await axios.post('http://localhost:5059/api/projectmember', leaderData);

            // Step 3: Add selected users as members
            const memberRequests = selectedUsers.value.map((user) => {
                return axios.post('http://localhost:5059/api/projectmember', {
                    userId: user.id,
                    projectId: projectId,
                    role: 'Member',
                });
            });

            await Promise.all(memberRequests);

            // Add Log
            const logResponse = await postLog(currentUserId, "Successfully add a new project");
            console.log('Log posted:', logResponse);

            // Success message
            successMessage.value = 'Project and members added successfully!';
            errorMessage.value = '';
            router.push(`/${currentUserId}/projects`); // Redirect after successful submission
        } catch (error) {
            console.error('Error:', error); // Log full error details
            console.error('Response data:', error.response?.data); // Log response from server
            successMessage.value = '';
            errorMessage.value = 'Failed to add project or members.';
        }
    };


    // Fetch users on component mount
    onMounted(() => {
        fetchUsers();
    });
</script>


<template>
    <div class="add-project-form">
        <h2>Add New Project</h2>

        <div v-if="errorMessage" class="error">{{ errorMessage }}</div>
        <div v-if="successMessage" class="success">{{ successMessage }}</div>

        <form @submit.prevent="addProject">
            <div class="form-group">
                <label for="name">Project Name</label>
                <input type="text" id="name" v-model="name" required placeholder="Enter project name" />
            </div>

            <div class="form-group">
                <label for="description">Description</label>
                <textarea id="description" v-model="description" required
                    placeholder="Enter project description"></textarea>
            </div>

            <div class="form-group">
                <label for="startDate">Start Date</label>
                <input type="date" id="startDate" v-model="startDate" required />
            </div>

            <div class="form-group">
                <label for="endDate">End Date</label>
                <input type="date" id="endDate" v-model="endDate" required />
            </div>

            <div class="form-group">
                <label for="status">Status</label>
                <select id="status" v-model="status" required>
                    <option value="active">Active</option>
                    <option value="inactive">In Active</option>
                    <option value="done">Done</option>
                </select>
            </div>

            <div class="form-group">
                <label for="members">Assigned Members</label>
                <div class="selected-members">
                    <!-- Display assigned members with a remove button -->
                    <div v-for="user in selectedUsers" :key="user.id" class="assigned-member-card">
                        <span>{{ user.name }} ({{ user.email }})</span>
                        <button type="button" class="remove-member-button" @click="removeUserFromProject(user.id)">
                            Remove
                        </button>
                    </div>
                </div>
                <div class="add-members-container">
                    <button type="button" class="add-members-button" @click="isModalOpen = true">
                        Add Members
                    </button>
                </div>
            </div>

            <div class="form-group">
                <button type="submit" class="submit-button">Add Project</button>
            </div>
        </form>

        <!-- Modal -->
        <div v-if="isModalOpen" class="modal">
            <div class="modal-content">
                <button class="close-button" @click="isModalOpen = false">
                    &times;
                </button>
                <h3>Select Users</h3>
                <input type="text" class="search-bar" placeholder="Search by email or name" v-model="searchQuery"
                    @input="searchUsers" />
                <div class="user-list">
                    <UserCard v-for="user in filteredUsers" :key="user.id" :user="user"
                        @click="addUserToProject(user)" />
                </div>
            </div>
        </div>
    </div>
</template>

<style scoped>
    .add-project-form {
        max-width: 700px;
        margin: 40px auto;
        padding: 30px;
        background-color: #f9f9f9;
        border-radius: 12px;
        box-shadow: 0 4px 12px rgba(0, 0, 0, 0.1);
    }

    h2 {
        text-align: center;
        font-size: 1.8rem;
        color: #333;
        margin-bottom: 20px;
    }

    .form-group {
        margin-bottom: 20px;
    }

    label {
        display: block;
        font-size: 1rem;
        color: #555;
        margin-bottom: 8px;
    }

    input,
    textarea,
    select {
        width: 100%;
        padding: 12px;
        font-size: 1rem;
        border: 1px solid #ddd;
        border-radius: 8px;
    }

    textarea {
        resize: vertical;
        height: 150px;
    }

    .submit-button,
    .add-members-button {
        padding: 10px 20px;
        font-size: 1rem;
        background-color: #4caf50;
        color: #fff;
        border: none;
        border-radius: 5px;
        cursor: pointer;
    }

    .submit-button:hover,
    .add-members-button:hover {
        background-color: #45a049;
    }

    .error,
    .success {
        text-align: center;
        font-weight: bold;
        margin-bottom: 20px;
    }

    .error {
        color: #e74c3c;
    }

    .success {
        color: #2ecc71;
    }

    .modal {
        position: fixed;
        top: 0;
        left: 0;
        right: 0;
        bottom: 0;
        background: rgba(0, 0, 0, 0.5);
        display: flex;
        justify-content: center;
        align-items: center;
        overflow: hidden;
    }

    .modal-content {
        position: relative;
        /* Important for the close button positioning */
        background: #fff;
        padding: 30px;
        border-radius: 8px;
        max-width: 700px;
        max-height: 80vh;
        overflow-y: auto;
        box-shadow: 0 4px 12px rgba(0, 0, 0, 0.2);
    }

    .close-button {
        position: absolute;
        top: 10px;
        right: 10px;
        background: none;
        border: none;
        font-size: 1.5rem;
        cursor: pointer;
        color: #555;
    }

    .close-button:hover {
        color: #000;
    }

    .search-bar {
        width: 100%;
        padding: 8px;
        margin-bottom: 15px;
        font-size: 1rem;
        border-radius: 6px;
        border: 1px solid #ddd;
    }

    .user-list {
        display: flex;
        flex-direction: column;
        gap: 10px;
    }

    .selected-members {
        display: flex;
        flex-direction: column;
        gap: 10px;
        margin-bottom: 20px;
    }

    .assigned-member-card {
        display: flex;
        justify-content: space-between;
        align-items: center;
        background-color: #f9f9f9;
        padding: 10px;
        border: 1px solid #ddd;
        border-radius: 8px;
    }

    .remove-member-button {
        background-color: #e74c3c;
        color: #fff;
        border: none;
        border-radius: 5px;
        padding: 5px 10px;
        font-size: 0.9rem;
        cursor: pointer;
    }

    .remove-member-button:hover {
        background-color: #c0392b;
    }
</style>
