const placeholder = '/coming-soon';
const ROUTES = {
    'home': process.env.NEXT_PUBLIC_BASE_PATH ? `${process.env.NEXT_PUBLIC_BASE_PATH}/` :'/',
    'appointments': '/appointments',
    'companyRegistration': '/register-company',
    'features': '/features',
    'about': '/about',
    'apptsmartSchedule': '/c/apptsmart/schedule',
    
    // TODO: Add these pages
    'contactUs': placeholder,
    'privacyPolicy': placeholder,
    'tos': placeholder,
    'sitemap': placeholder,
    'support': placeholder,
    'facebook': placeholder,
    'instagram': placeholder,
    'twitter': placeholder
}

export default ROUTES;