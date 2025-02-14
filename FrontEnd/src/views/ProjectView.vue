<script setup>
    import { ref, onMounted } from 'vue';
    import axios from 'axios';
    import ProjectCard from '@/components/ProjectCard.vue';
    import { RouterLink } from 'vue-router';
    import postLog from '@/logs/postLog';

    const userId = localStorage.getItem('userId');
    const projects = ref([]);
    const errorMessage = ref('');

    const fetchUserProjects = async () => {
        try {
            const response = await axios.get(`http://localhost:5059/api/ProjectMember/user/${userId}`);
            projects.value = response.data.map((item) => ({
                ...item.project,
                userRole: item.userRole,
            }));
        } catch (error) {
            errorMessage.value = 'Failed to load projects. Please try again later.';
            console.error(error);
        }
    };

    const deleteProject = async (projectId) => {
        try {
            const response = await axios.delete(`http://localhost:5059/api/Project/${projectId}`);
            projects.value = projects.value.filter(project => project.id !== projectId);
            console.log(`Project with ID ${projectId} deleted successfully.`);
            // Add Log
            const logResponse = await postLog(userId, "Deleted a project");
            console.log('Log posted:', logResponse);
        } catch (error) {
            console.error(`Failed to delete project with ID ${projectId}:`, error);
            alert('Failed to delete project. Please try again later.');
        }
    };

    onMounted(() => {
        fetchUserProjects();
    });
</script>

<template>
    <div class="project-list-container p-6">
        <!-- Error Message -->
        <div v-if="errorMessage" class="error-message text-red-500 text-center mb-6">
            {{ errorMessage }}
        </div>

        <!-- Project Grid -->
        <div class="grid grid-cols-4 gap-6">
            <ProjectCard v-for="project in projects" :key="project.id" :projectId="project.id" :groupCode="project.name"
                :groupName="project.description">
                <template #image>
                    <div
                        class="h-full w-full bg-gray-200 flex items-center justify-center text-gray-400 text-xl font-semibold">
                        {{ project.name.charAt(0).toUpperCase() }}
                    </div>
                </template>
                <template #footer>
                    <div class="button-group flex justify-center items-center space-x-2">
                        <!-- View Details Button -->
                        <RouterLink :to="`/${userId}/projects/project-id=${project.id}/view`"
                            class="btn-primary router-link-button">
                            Detail
                        </RouterLink>
                        <!-- Edit Button (conditionally rendered) -->
                        <RouterLink :to="`/${userId}/projects/project-id=${project.id}/edit`"
                            v-if="project.userRole === 'Leader'" class="btn-warning router-link-button">
                            Edit
                        </RouterLink>

                        <!-- Remove Button (conditionally rendered) -->
                        <button v-if="project.userRole === 'Leader'" class="btn-danger"
                            @click="deleteProject(project.id)">
                            Remove
                        </button>
                    </div>
                </template>
            </ProjectCard>
        </div>
    </div>
</template>

<style scoped>
    .project-list-container {
        max-width: 1200px;
        margin: 0 auto;
        padding: 1rem;
    }

    .grid {
        display: grid;
        grid-template-columns: repeat(4, 1fr);
        gap: 1.5rem;
    }

    .error-message {
        font-size: 1.2rem;
        font-weight: bold;
    }

    button {
        padding: 0.5rem 1rem;
        border-radius: 0.375rem;
        font-size: 1rem;
        font-weight: 600;
        text-align: center;
        transition: all 0.3s ease;
        text-shadow: 1px 1px 2px rgba(0, 0, 0, 0.5);
        display: inline-block;
    }

    .btn-primary {
        background-color: #2563eb;
        color: white;
    }

    .btn-primary:hover {
        background-color: #1d4ed8;
    }

    .btn-warning {
        background-color: #facc15;
        color: white;
    }

    .btn-warning:hover {
        background-color: #eab308;
    }

    .btn-danger {
        background-color: #dc2626;
        color: white;
    }

    .btn-danger:hover {
        background-color: #b91c1c;
    }

    /* Additional styles to make RouterLink act like a button */
    .router-link-button {
        display: inline-block;
        padding: 0.5rem 1rem;
        border-radius: 0.375rem;
        font-size: 1rem;
        font-weight: 600;
        text-align: center;
        transition: all 0.3s ease;
        text-shadow: 1px 1px 2px rgba(0, 0, 0, 0.5);
        text-decoration: none;
    }

    .router-link-button:hover {
        cursor: pointer;
    }

    /* Ensure button group aligns its items vertically in the center */
    .button-group {
        display: flex;
        justify-content: center;
        align-items: center;
        gap: 0.5rem;
    }
</style>
