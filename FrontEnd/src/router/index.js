import { createRouter, createWebHistory } from "vue-router";
import axios from "axios";
import SignInView from "@/views/SignInView.vue";
import SignUpView from "@/views/SignUpView.vue";
import HomeView from "@/views/HomeView.vue";
import NotFoundView from "@/views/NotFoundView.vue";
import InAppView from "@/views/AllTaskView.vue";
import FiveDayTaskView from "@/views/FiveDayTaskView.vue";
import MyDayView from "@/views/MyDayView.vue";
import AccountView from "@/views/AccountView.vue";
import ChangePassword from "@/views/ChangePassword.vue";
import AddProjectView from "@/views/AddProjectView.vue";
import ProjectView from "@/views/ProjectView.vue";
import EditProjectView from "@/views/EditProjectView.vue";
import ProjectDetailView from "@/views/ProjectDetailView.vue";
import ProjectTaskView from "@/views/ProjectTaskView.vue";
import NoticeView from "@/views/NoticeView.vue";

// Helper function for checking authentication
const isAuthenticated = () => {
    const token = localStorage.getItem('token');
    const userId = localStorage.getItem('userId');
    return { token, userId };
};

// Global route guards for authenticated views
const authGuard = (to, from, next) => {
    const { token, userId } = isAuthenticated();
    if (!token) {
        next('/signin'); // Redirect to sign-in if no token
    } else if (!to.params.id || userId !== to.params.id) {
        next(`/${userId}/all-my-tasks`); // Redirect to the correct userId
    } else {
        next(); // Allow navigation if authenticated
    }
};


const redirectIfAuthenticated = (to, from, next) => {
    const { token, userId } = isAuthenticated();
    if (token) {
        next(`/${userId}/all-my-tasks`); // Redirect to tasks if authenticated
    } else {
        next(); // Allow navigation if not authenticated
    }
};
const authProjectMember = async (to, from, next) => {
    const { token, userId } = isAuthenticated();
    const routeUserId = to.params.userId;

    if (!token) {
        return next('/signin'); // Redirect to sign-in if no token
    }

    if (routeUserId !== userId) {
        return next(`/${userId}/all-my-tasks`); // Redirect if the userId in the route doesn't match the logged-in user
    }

    try {
        // Check if the user is the leader of the project
        const response = await axios.get(`http://localhost:5059/api/ProjectMember/project/${to.params.projectId}/user/${userId}`);
        // If the user is the leader of the project, proceed
        if (response.data.role === 'Leader' || response.data.role === 'Member') {
            next(); // Allow navigation
        } else {
            next(`/${userId}/all-my-tasks`); // Redirect if not a leader
        }
    } catch (error) {
        console.error(error);
        next(`/${userId}/all-my-tasks`); // Redirect if error in API call
    }
};
const authProject = async (to, from, next) => {
    const { token, userId } = isAuthenticated();
    const routeUserId = to.params.userId;

    if (!token) {
        return next('/signin'); // Redirect to sign-in if no token
    }

    if (routeUserId !== userId) {
        return next(`/${userId}/all-my-tasks`); // Redirect if the userId in the route doesn't match the logged-in user
    }

    try {
        // Check if the user is the leader of the project
        const response = await axios.get(`http://localhost:5059/api/ProjectMember/project/${to.params.projectId}/user/${userId}`);
        // If the user is the leader of the project, proceed
        if (response.data.role === 'Leader') {
            next(); // Allow navigation
        } else {
            next(`/${userId}/all-my-tasks`); // Redirect if not a leader
        }
    } catch (error) {
        console.error(error);
        next(`/${userId}/all-my-tasks`); // Redirect if error in API call
    }
};

// Define the routes
const routes = [
    {
        path: '/',
        name: 'home',
        component: HomeView,
        meta: { hideSideBar: true },
    },
    {
        path: '/signin',
        name: 'Sign in',
        component: SignInView,
        meta: { hideSideBar: true },
        beforeEnter: redirectIfAuthenticated,
    },
    {
        path: '/signup',
        name: 'Sign up',
        component: SignUpView,
        meta: { hideSideBar: true },
        beforeEnter: redirectIfAuthenticated,
    },
    {
        path: '/:id/all-my-tasks',
        name: 'All my tasks',
        component: InAppView,
        meta: { hideNavBar: true },
        beforeEnter: authGuard,
    },
    {
        path: '/:id/next-5-days',
        name: 'Next 5 days',
        component: FiveDayTaskView,
        meta: { hideNavBar: true },
        beforeEnter: authGuard,
    },
    {
        path: '/:id/my-day',
        name: 'My Day',
        component: MyDayView,
        meta: { hideNavBar: true },
        beforeEnter: authGuard,
    },
    {
        path: '/:id/account',
        name: 'account',
        component: AccountView,
        meta: { hideNavBar: true },
        beforeEnter: authGuard,
    },
    {
        path: '/:id/change-password',
        name: 'change-password',
        component: ChangePassword,
        meta: { hideNavBar: true },
        beforeEnter: authGuard,
    },
    {
        path: '/:id/notices',
        name: 'notices',
        component: NoticeView,
        meta: { hideNavBar: true },
        beforeEnter: authGuard,
    },
    {
        path: '/:id/add-project',
        name: 'add-project',
        component: AddProjectView,
        meta: { hideNavBar: true },
        beforeEnter: authGuard,
    },
    {
        path: '/:id/projects',
        name: 'projects',
        component: ProjectView,
        meta: { hideNavBar: true },
        beforeEnter: authGuard,
    },
    {
        path: '/:userId/projects/project-id=:projectId/edit',
        name: 'edit-project',
        component: EditProjectView,
        meta: { hideNavBar: true },
        beforeEnter: authProject,
    },
    {
        path: '/:userId/projects/project-id=:projectId/view',
        name: 'view-project',
        component: ProjectDetailView,
        meta: { hideNavBar: true },
        beforeEnter: authProjectMember,
    },
    {
        path: '/:userId/projects/project-id=:projectId/tasks/memberId=:memberId',
        name: 'project-task',
        component: ProjectTaskView,
        meta: { hideNavBar: true },
        beforeEnter: authProjectMember,
    },
    {
        path: '/:catchAll(.*)',
        name: 'not-found',
        component: NotFoundView,
        meta: { hideSideBar: true },
    },
];

// Create the router instance
const router = createRouter({
    history: createWebHistory(import.meta.env.BASE_URL),
    routes,
});

// Export the router
export default router;